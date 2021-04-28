#include <Madgwick_Quat.h>

// https://www.arduino.cc/en/Tutorial/Genuino101CurieIMUOrientationVisualiser

// unity : https://answers.unity.com/questions/1201108/quaternion-from-imu-sensor-to-gameobject-orientati.html

// more unity: https://answers.unity.com/questions/1163599/verifying-euler-to-quaternion-calculations.html?childToView=1163849#answer-1163849

// https://iotdk.intel.com/docs/master/upm/classupm_1_1_curie_imu.html

#include <CurieIMU.h>
#include <Madgwick_Quat.h>

Madgwick_Quat filter;
unsigned long microsPerReading, microsPrevious;
float accelScale, gyroScale;

void setup() {
  Serial.begin(9600);

  // start the IMU and filter
  CurieIMU.begin();
  CurieIMU.setGyroRate(25);
  CurieIMU.setAccelerometerRate(25);
  filter.begin(25);

  // Set the accelerometer range to 2G
  CurieIMU.setAccelerometerRange(2);
  // Set the gyroscope range to 250 degrees/second
  CurieIMU.setGyroRange(250);

  // initialize variables to pace updates to correct rate
  microsPerReading = 1000000 / 25;
  microsPrevious = micros();
}

void loop() {
  int aix, aiy, aiz;
  int gix, giy, giz;
  float ax, ay, az;
  float gx, gy, gz;
  float roll, pitch, heading;
  unsigned long microsNow;

  // check if it's time to read data and update the filter
  microsNow = micros();
  if (microsNow - microsPrevious >= microsPerReading) {

    // read raw data from CurieIMU
    CurieIMU.readMotionSensor(aix, aiy, aiz, gix, giy, giz);

    // convert from raw data to gravity and degrees/second units
    ax = convertRawAcceleration(aix);
    ay = convertRawAcceleration(aiy);
    az = convertRawAcceleration(aiz);
    gx = convertRawGyro(gix);
    gy = convertRawGyro(giy);
    gz = convertRawGyro(giz);

    // update the filter, which computes orientation
    filter.updateIMU(gx, gy, gz, ax, ay, az);  // original
    
    // print the heading, pitch and roll
//    roll = filter.getRoll();
//    pitch = filter.getPitch();
//    heading = filter.getYaw();
//    Serial.print("Orientation: ");
//    Serial.print(heading);
//    Serial.print(" ");
//    Serial.print(pitch);
//    Serial.print(" ");
//    Serial.println(roll);
//====================

//      Serial.print("quat: ");
      Serial.print("AB ");
      Serial.print(filter.getQ0(), DEC);
//      Serial.print(", ");
      Serial.print(filter.getQ1(), DEC);
//      Serial.print(", ");
      Serial.print(filter.getQ2(), DEC);
//      Serial.print(", ");
      Serial.println(filter.getQ3(), DEC);
      
    // increment previous time, so we keep proper pace
    microsPrevious = microsPrevious + microsPerReading;
  }
}

float convertRawAcceleration(int aRaw) {
  // since we are using 2G range
  // -2g maps to a raw value of -32768
  // +2g maps to a raw value of 32767
  
  float a = (aRaw * 2.0) / 32768.0;
  return a;
}

float convertRawGyro(int gRaw) {
  // since we are using 250 degrees/seconds range
  // -250 maps to a raw value of -32768
  // +250 maps to a raw value of 32767
  
  float g = (gRaw * 250.0) / 32768.0;
  return g;
}
