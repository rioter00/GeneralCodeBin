/*
 * Copyright (c) 2016 Intel Corporation.  All rights reserved.
 * See the bottom of this file for the license terms.
 */

/*
   This sketch example demonstrates how the BMI160 on the
   Intel(R) Curie(TM) module can be used to read accelerometer data
   and translate it to an orientation
*/

#include "CurieIMU.h"

int lastOrientation = - 1; // previous orientation (for comparison)

void setup() {
  Serial.begin(9600); // initialize Serial communication
  while (!Serial);    // wait for the serial port to open

  // initialize device
  Serial.println("Initializing IMU device...");
  CurieIMU.begin();

  // Set the accelerometer range to 2G
  CurieIMU.setAccelerometerRange(2);
}

void loop() {
int orientation = - 1;   // the board's orientation
  String orientationString; // string for printing description of orientation
  /*
    The orientations of the board:
    0: flat, processor facing up -- "back up"
    1: flat, processor facing down -- "belly up"
    2: landscape, analog pins down -- "left up"
    3: landscape, analog pins up -- "right up"
    4: portrait, USB connector up -- "tail up"
    5: portrait, USB connector down -- "head up"
  */
  // read accelerometer:
  int x = CurieIMU.readAccelerometer(X_AXIS);
  int y = CurieIMU.readAccelerometer(Y_AXIS);
  int z = CurieIMU.readAccelerometer(Z_AXIS);

//  Serial.print("x");
//  Serial.print(x);
//  Serial.print(" y: ");
//  Serial.println(y);
  Serial.print(" z: ");
  Serial.println(z);
  delayMicroseconds(5000);

//  // calculate the absolute values, to determine the largest
//  int absX = abs(x);
//  int absY = abs(y);
//  int absZ = abs(z);
//
//  if ( (absZ > absX) && (absZ > absY)) {
//    // base orientation on Z
//    if (z > 0) {
//      orientationString = "back up";
//      orientation = 0;  
//    } else {
//      orientationString = "belly up";
//      orientation = 1;
//    }
//  } else if ( (absY > absX) && (absY > absZ)) {
//    // base orientation on Y
//    if (y > 0) {
//      orientationString = "left up";
//      orientation = 2;
//    } else {
//      orientationString = "right up";
//      orientation = 3;
//    }
//  } else {
//    // base orientation on X
//    if (x < 0) {
//      orientationString = "tail up";
//      orientation = 4;
//    } else {
//      orientationString = "head up";
//      orientation = 5;
//    }
//  }

//  // if the orientation has changed, print out a description:
//  if (orientation != lastOrientation) {
//    Serial.println(orientationString);
//    lastOrientation = orientation;
//  }
}

/*
   Copyright (c) 2016 Intel Corporation.  All rights reserved.

   This library is free software; you can redistribute it and/or
   modify it under the terms of the GNU Lesser General Public
   License as published by the Free Software Foundation; either
   version 2.1 of the License, or (at your option) any later version.

   This library is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
   Lesser General Public License for more details.

   You should have received a copy of the GNU Lesser General Public
   License along with this library; if not, write to the Free Software
   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA

*/
