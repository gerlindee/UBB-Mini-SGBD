﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public static class Validator
    {

        public static string EMPTY_TABLE_NAME = "Table name cannot be empty!";
        public static string EMPTY_COLUMN_NAME = "Column name cannot be empty!";
        public static string MAX_CHAR_TABLE_NAME = "Table name cannot have more than 100 characters!";
        public static string NEGATIVE_LENGTH = "";
        public static string MAX_LENGTH = "Lenght cannot be greater than 1000 charachters";
        public static string INVALID_LENGHT_USE = "Date types cannot use lenght!";
        public static string LENGHT_NUMERICAL = "Numerical lenght required";

        //insert
        public static string EMPTY_FIELD = "Field value cannot be empty!";
        public static string EXCEEDED_LENGHT = "Length exceeded!";
        public static string WRONG_TYPE = "Type not respected!";

        public static bool isStringEmpty(string str)
        {
            return str.Equals("");
        }

        public static bool isStringMaxLenght(string str)
        {
            return str.Length > 100;
        }
        public static bool isLengthMax(string str)
        {
            int val;
            
            if (int.TryParse(str, out val)) 
            { return val > 1000; }

            return false;
        }
        public static bool isLengthCorrect(string str)
        {
            int val;
            return int.TryParse(str, out val);
        }
        public static bool isLenghtExceeded(int lenght, int obj)
        {
            return obj > lenght; 
        }
        public static bool isTypeCorrect(string type, string value)
        {
            if (type == "INT" || type == "BOOLEAN" || type == "DOUBLE" || type == "FLOAT")
                return int.TryParse(value, out int val);
            if (type == "DATE" || type == "TIME" || type == "DATETIME")
                return (DateTime.TryParse(value, out var result) == true);
            return true;
        }
    }
}
