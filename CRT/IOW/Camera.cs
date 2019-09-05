using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CRT.IOW
{
    public class Camera
    {
        public Vec3 lower_left_corner;
        public Vec3 horizontal;
        public Vec3 vertical;

        public Vec3 origin;
        public Vec3 lookAt;
        public Vec3 vup;
        public double vfov;
        public double aspect;

        Matrix4x4 cameraToWorld;

        public Camera(Vec3 lookFrom, Vec3 lookAt, Vec3 vup, double vfov, double aspect)
        {
            origin = lookFrom;
            this.lookAt = lookAt;
            this.vup = vup;
            this.vfov = vfov;
            this.aspect = aspect;
            updatePos();
        }

        public void update()
        {
            doMove();
            doLook();
        }

        private void doMove()
        {
            Vec3 movement = new Vec3();
            double speed = 0.10;
            bool changed = false;

            if (Program.input.IsKeyHeld(OpenTK.Input.Key.W))
            {
                movement += (lookAt - origin) * speed;
                changed = true;
            }

            if (Program.input.IsKeyHeld(OpenTK.Input.Key.S))
            {
                movement -= (lookAt - origin) * speed;
                changed = true;
            }

            if (Program.input.IsKeyHeld(OpenTK.Input.Key.D))
            {
                movement -= Vec3.cross(vup, (lookAt - origin)) * speed;
                changed = true;
            }

            if (Program.input.IsKeyHeld(OpenTK.Input.Key.A))
            {
                movement += Vec3.cross(vup, (lookAt - origin)) * speed;
                changed = true;
            }

            if (changed)
            {
                origin += movement;
                lookAt += movement;
                updatePos();
            }
        }

        private void doLook()
        {
            Vec3 strafe = new Vec3();
            double strafeSpeed = 0.08;
            bool changed = false;

            if (Program.input.IsKeyHeld(OpenTK.Input.Key.Up))
            {
                strafe.x -= strafeSpeed / 2;
                changed = true;
            }

            if (Program.input.IsKeyHeld(OpenTK.Input.Key.Down))
            {
                strafe.x += strafeSpeed / 2;
                changed = true;
            }

            if (Program.input.IsKeyHeld(OpenTK.Input.Key.Left))
            {
                strafe.y -= strafeSpeed;
                changed = true;
            }

            if (Program.input.IsKeyHeld(OpenTK.Input.Key.Right))
            {
                strafe.y += strafeSpeed;
                changed = true;
            }

            if (Program.input.IsKeyHeld(OpenTK.Input.Key.Q))
            {
                vfov -= 1;
                changed = true;
            }

            if (Program.input.IsKeyHeld(OpenTK.Input.Key.E))
            {
                vfov += 1;
                changed = true;
            }

            if (changed)
            {
                Vector4 temp = lookAt - origin;

                if(strafe.y != 0)
                {
                    temp += Vector4.Transform(temp, Matrix4x4.CreateFromAxisAngle(Vec3.cross(Vec3.cross(vup, (lookAt - origin)), (lookAt - origin)), (float)strafe.y));
                }
                if (strafe.x != 0)
                {
                    temp += Vector4.Transform(temp, Matrix4x4.CreateFromAxisAngle(Vec3.cross(vup, (lookAt - origin)), (float)strafe.x));
                }
                
                lookAt = origin + Vec3.unitVector(temp);

                updatePos();
            }
        }

        public void updatePos()
        {
            Vec3 u = new Vec3();
            Vec3 v = new Vec3();
            Vec3 w = new Vec3();

            double theta = vfov * Math.PI / 180;
            double half_height = Math.Tan(theta / 2.0);
            double half_width = aspect * half_height;

            w = Vec3.unitVector(origin - (lookAt));
            u = Vec3.unitVector(Vec3.cross(vup, w));
            v = Vec3.cross(w, u);

            lower_left_corner = new Vec3(-half_width, -half_height, -1.0);
            lower_left_corner = origin - half_width * u - half_height * v - w;
            horizontal = 2 * half_width * u;
            vertical = 2 * half_height * v;
        }

        public Ray GetRay(double u, double v)
        {
            return new Ray(origin, lower_left_corner + (u * horizontal) + (v * vertical) - origin);
        }
    }
}
