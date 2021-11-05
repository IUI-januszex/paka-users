using System;
using System.Collections.Generic;

namespace PakaUsers.Model
{
    public class Courier : Worker
    {
        public override UserType UserType => UserType.Courier;
    }
}