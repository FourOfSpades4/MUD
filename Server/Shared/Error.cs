using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUD
{
    public enum ErrorType
    {
        INVALID_USERNAME,
        INVALID_PASSWORD,
        DIRECTION_DOES_NOT_EXIST,

    }
    public class Error : IDarkRiftSerializable
    {
        public ErrorType Type { get; private set; }

        public static Error CreateError(ErrorType type)
        {
            Error e = new Error();
            e.Type = type;
            return e;
        }

        public virtual void Deserialize(DeserializeEvent e)
        {
            Type = (ErrorType)e.Reader.ReadInt32();
        }

        public virtual void Serialize(SerializeEvent e)
        {
            e.Writer.Write((int)Type);
        }
    }
}
