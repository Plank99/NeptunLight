﻿using System;
using System.Text.RegularExpressions;

namespace NeptunLight.Models
{
    public struct Semester
        : IComparable<Semester>
    {
        #region Equality members

        public bool Equals(Semester other)
        {
            return UniqueId == other.UniqueId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Semester && Equals((Semester) obj);
        }

        public override int GetHashCode()
        {
            return UniqueId;
        }

        public static bool operator==(Semester left, Semester right)
        {
            return left.Equals(right);
        }

        public static bool operator!=(Semester left, Semester right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region Relational members

        public int CompareTo(Semester other)
        {
            return UniqueId.CompareTo(other.UniqueId);
        }

        public static bool operator<(Semester left, Semester right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator>(Semester left, Semester right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator<=(Semester left, Semester right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator>=(Semester left, Semester right)
        {
            return left.CompareTo(right) >= 0;
        }

        #endregion


        public Semester(int year, int semester)
        {
            if (semester > 2 || semester < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(semester), "Semester number can either be 1 or 2");
            }
            PrimaryYear = year;
            SemesterNumber = semester;
        }

        public int PrimaryYear { get; }

        public int SecondaryYear => PrimaryYear + 1;

        public int SemesterNumber { get; }

        public string Name => $"{PrimaryYear}/{(PrimaryYear + 1).ToString().Substring(2)}/{SemesterNumber}";

        public int UniqueId => PrimaryYear * 10 + SemesterNumber;

        public override string ToString()
        {
            return Name;
        }

        private static readonly Regex Parser = new Regex(@"([0-9]{4})\/[0-9]{2}\/([12])");

        public static Semester Parse(string s)
        {
            Match result = Parser.Match(s);
            if (!result.Success)
                throw new FormatException("Unable to parse semester.");

            return new Semester(Convert.ToInt32(result.Groups[0].Value), Convert.ToInt32(result.Groups[1].Value));
        }
    }
}