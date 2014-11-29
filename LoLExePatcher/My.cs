﻿using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;

namespace LoLExePatcher
{
    public static class My
    {
        static private Computer __Computer = new Computer();
        static private WindowsFormsApplicationBase __Application = new WindowsFormsApplicationBase();
        static private User __User = new User();

        public static ServerComputer Computer
        {
            get
            {
                return __Computer;
            }
        }

        public static WindowsFormsApplicationBase Application
        {
            get
            {
                return __Application;
            }
        }

        public static User User
        {
            get
            {
                return __User;
            }
        }
    }
}
