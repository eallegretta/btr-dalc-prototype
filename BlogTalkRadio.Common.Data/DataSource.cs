using System;
using Cinchcast.Framework;

namespace BlogTalkRadio.Common.Data
{
    public abstract class DataSource: IEquatable<DataSource>
    {
        public string Identifier { get; private set; }

        public DataSource(string identifier)
        {
            Identifier = identifier;
        }

        public bool Equals(DataSource other)
        {
            return Equals((object) other);
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(Identifier);
        }

        public override bool Equals(object obj)
        {
            if (! (obj is DataSource))
            {
                return false;
            }

            return GetHashCode() == obj.GetHashCode();
        }

        public static bool operator ==(DataSource dataSource1, DataSource dataSource2)
        {
            if (ReferenceEquals(dataSource1, null))
            {
                return ReferenceEquals(dataSource2, null);
            }

            return dataSource1.Equals(dataSource2);
        }

        public static bool operator !=(DataSource dataSource1, DataSource dataSource2)
        {
            return !Equals(dataSource1, dataSource2);
        }
    }
}