using System;
using System.Runtime.Serialization;

namespace Device{

    public class InitalizationFailedException : Exception{


        public InitalizationFailedException() : base("Initialization of Device failed!"){}

        public InitalizationFailedException(string message) : base(message){}

        public InitalizationFailedException(string message, Exception innerException) : base(message, innerException){}

        public InitalizationFailedException(SerializationInfo info, StreamingContext context) : base(info, context){}


    }
}