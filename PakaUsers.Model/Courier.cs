using System;
using System.Collections.Generic;

namespace PakaUsers.Model
{
    public class Courier : Worker
    {
        public List<long> Parcels { get; set; }

        public override UserType UserType => UserType.Courier;
    }
}