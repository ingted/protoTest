using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gRTB
{
    public class ProtoMessageConverter : JsonConverter
    {
        /// <summary>
        /// Called by NewtonSoft.Json's method to ask if this object can serialize
        /// an object of a given type.
        /// </summary>
        /// <returns>True if the objectType is a Protocol Message.</returns>
        public override bool CanConvert(System.Type objectType)
        {
            return typeof(Google.Protobuf.IMessage)
                .IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Reads the json representation of a Protocol Message and reconstructs
        /// the Protocol Message.
        /// </summary>
        /// <param name="objectType">The Protocol Message type.</param>
        /// <returns>An instance of objectType.</returns>
        public override object ReadJson(JsonReader reader,
            System.Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            // The only way to find where this json object begins and ends is by
            // reading it in as a generic ExpandoObject.
            // Read an entire object from the reader.
            var converter = new ExpandoObjectConverter();
            object o = converter.ReadJson(reader, objectType, existingValue,
                serializer);
            // Convert it back to json text.
            string text = JsonConvert.SerializeObject(o);
            // And let protobuf's parser parse the text.
            IMessage message = (IMessage)Activator
                .CreateInstance(objectType);
            return Google.Protobuf.JsonParser.Default.Parse(text,
                message.Descriptor);
        }

        /// <summary>
        /// Writes the json representation of a Protocol Message.
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            // Let Protobuf's JsonFormatter do all the work.
            writer.WriteRawValue(Google.Protobuf.JsonFormatter.Default
                .Format((IMessage)value));
        }
    }
}
