using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine{
	public class CarBehaviour {

		private int mass; // vehicle mass (kg)
		private float gravity; // acceleration due to gravity m/s2
		private float length; //full length from front to back
		private float a, b ; //a- length from centre to front; b - length from centre to back
		private int I; //moment of intertia
		private double forwardVelocity; //forward velocity
		private double steerAngleRadians;
		private	double Fz0; //nominal load
		private Tyre tyre;
		private Forces forces;
		private Movement movement;
		private double yawVelocityRadians;
		private double lateralVelocity;

		// Use this for initialization
		public CarBehaviour(double steerAngleRadians) {
			mass = 1150;
			gravity = 9.81;
			length = 2.66;
			a = 1.06;
			b = length - a;
			I = 1850;
			forwardVelocity = 80 / 3.6;
			yawVelocityRadians = lateralVelocity = Math.Abs(double.MinValue);

			this.steerAngleRadians = steerAngleRadians;

			//Object creation initialization
			
			this.tyre = new Tyre (mass, gravity, length, steerAngleRadians, yawVelocityRadians, lateralVelocity, a, Fz0);
			this.forces = new Forces (this.tyre.tyreForceFront(), this.tyre.tyreForceRear(), this.a, this.b);
			this.movement = new Movement (this.forces.FyTotal, this.forces.MzMoment, this.I, this.forwardVelocity); 
		}

		public Dictionary<string, double> Run()
		{
			tyre.YawVelocity = movement.yawVelocity();
			tyre.LateralVelocity = movement.lateralVelocity();
			//tyre.SteerAngle = Wheel Axis Input in radians

			forces.Fy1 = tyre.tyreForceFront();
			forces.Fy2 = tyre.tyreForceRear();

			movement.FyTotal = forces.FyTotal();
			movement.MzTotal = forces.MzMoment();

			Dictionary<string, double> dictionary = new Dictionary<string, double>();
			dictionary.Add ("acc_y", this.acceleration());
			dictionary.Add ("yaw_velocity", this.yawVelocity ());
			dictionary.Add ("steerAngle_degrees", this.steerAngleDegrees ());

			return dictionary;
		}

		public double acceleration()
		{
			double n1 = movement.accelerationY + (movement.yawVelocity * forwardVelocity);
			double n2 = n1 / gravity;

			return n2;
		}

		public double yawVelocity()
		{
			return movement.yawVelocity * (180 / Math.PI);
		}

		public double steerAngleDegrees()
		{
			double n1 = Math.Atan(movement.lateralVelocity / forwardVelocity);
			double n2 = n1 * (180 / Math.PI);

			return n2;

		}
		public Point calculatePositionChange()
		{
			double x = (forwardVelocity * Math.Cos(steerAngleDegrees)) - (lateralVelocity * Math.Sin(steerAngleDegrees));
			double y = (forwardVelocity * Math.Sin (steerAngleDegrees)) - (lateralVelocity * Math.Cos(steerAngleDegrees));

			return new Point (x, y);
		}
	}
}
