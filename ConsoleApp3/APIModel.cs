using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp3
{
    public class APIBaseResponseModel
    {
        public int code;
        public string next;
        public Character[] results;
    }


    public class Character
	{
        public string name;
        public string[] films;
	}


}
