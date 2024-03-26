using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using SceneModel;

namespace SceneServices
{
    public static class BinarySerialization
    {

        private static void WriteString(this BinaryWriter writer, string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            writer.Write(bytes.Length);
            writer.Write(bytes,0, bytes.Length);
        }

        private static void Write(this BinaryWriter writer, ICollection<string> strings)
        {
            writer.Write(strings.Count);
            foreach (string str in strings)
            {
                writer.WriteString(str);
            }
        }

        private static void Write(this BinaryWriter writer, Participant p)
        {
            writer.WriteString(p.Skeleton);
            writer.Write((int)p.Sex);
            writer.Write(p.IsAggressor);
            writer.Write(p.IsVictim);
            // TODO: other attributes
        }

        private static void Write(this BinaryWriter writer, IList<Participant> participants)
        {
            writer.Write(participants.Count);
            for (int i = 0; i < participants.Count; i++)
            {
                writer.Write(participants[i]);
            }
        }

        private static void Write(this BinaryWriter writer, ParticipantContactDetails details)
        {
            writer.Write(details.ParticipantIndex);
            writer.WriteString(details.Contact.Id);
            writer.WriteString(details.ReversePath ?? string.Empty);
            writer.Write(details.Stimulation);
            writer.Write(details.Hold);
            writer.Write(details.Pain);
            writer.Write(details.Comfort);
            writer.Write(details.Tickle);
            writer.Write((int)details.PainType);
        }

        private static void Write(this BinaryWriter writer, EnvironmentContact contact)
        {
            writer.Write((int)contact.Direction);
            writer.Write(contact.Details);
        }

        private static void Write(this BinaryWriter writer, ActorsContact contact)
        {
            writer.Write(contact.From);
            writer.Write(contact.To);
        }

        private static void Write(this BinaryWriter writer, ICollection<EnvironmentContact> contacts)
        {
            writer.Write(contacts.Count);
            foreach (var contact in contacts)
            {
                writer.Write(contact);
            }
        }

        private static void Write(this BinaryWriter writer, ICollection<ActorsContact> contacts)
        {
            writer.Write(contacts.Count);
            foreach (var contact in contacts)
            {
                writer.Write(contact);
            }
        }

        private static void Write(this BinaryWriter writer, JValue value)
        {
            writer.Write((int)value.Type);
            switch (value.Type)
            {
                case JTokenType.Null:
                    return;
                case JTokenType.Integer:
                    writer.Write(Convert.ToInt32(value.Value));
                    return;
                case JTokenType.Boolean:
                    writer.Write(Convert.ToBoolean(value.Value));
                    return;
                case JTokenType.Float:
                    writer.Write(Convert.ToSingle(value.Value));
                    return;
                case JTokenType.String:
                    writer.WriteString(Convert.ToString(value.Value));
                    return;
                default:
                    throw new NotImplementedException($"Unsupported JSON value type {value.Type}. [{(JToken)value.Parent ?? value}]");
            }
        }

        private static void WriteToken(this BinaryWriter writer, JToken item)
        {
            switch (item.Type)
            {
                case JTokenType.Object:
                    writer.Write((JObject)item);
                    return;
                case JTokenType.Array:
                    writer.Write((JArray)item);
                    return;
                default:
                    writer.Write((JValue)item);
                    return;
            }
        }

        private static void Write(this BinaryWriter writer, JArray array)
        {
            writer.Write((int)JTokenType.Array);
            writer.Write(array.Count);
            for (int i = 0; i < array.Count; i++)
            {
                var item = array[i];
                writer.WriteToken(item);
            }
        }

        private static void Write(this BinaryWriter writer, JProperty property)
        {
            writer.Write((int)JTokenType.Property);
            writer.WriteString(property.Name);
            writer.WriteToken(property.Value);
        }

        private static void Write(this BinaryWriter writer, JObject jo)
        {
            writer.Write((int)JTokenType.Object);
            writer.Write(jo.Count);
            foreach (JProperty property in jo.Properties())
            {
                writer.Write(property);
            }
        }

        public static void Write(this Stream stream, Scene scene)
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                writer.WriteString(scene.Id);
                writer.Write((int)scene.Type);
                writer.Write(scene.Furniture);
                writer.Write(scene.Participants);
                writer.Write(scene.ActorsContacts);
                writer.Write(scene.EnvironmentContacts);

                writer.Write(scene.Tags ?? Array.Empty<string>());

                writer.Write(scene.Authors ?? Array.Empty<string>());
                writer.Write(scene.Narrative ?? Array.Empty<string>());
                writer.Write(scene.Feeling ?? Array.Empty<string>());
                writer.Write(scene.Service ?? Array.Empty<string>());
                writer.Write(scene.Attribute ?? Array.Empty<string>());
                writer.Write(scene.Other ?? Array.Empty<string>());

                writer.Write(scene.GetCustomAttributes());
            }
        }

        public static void Write(this Stream stream, ICollection<Scene> scenes)
        {
            var writer = new BinaryWriter(stream, Encoding.UTF8, true);
            writer.Write(scenes.Count);
            foreach (var scene in scenes)
            {
                stream.Write(scene);
            }
            writer.Write((int)0); // errors count
        }

        public static void WriteErrors(this Stream stream, params string[] messages)
        {
            var writer = new BinaryWriter(stream, Encoding.UTF8, true);
            writer.Write((int)0); // scenes count
            writer.Write(messages);
        }

        public static byte[] ToByteArray(this Scene scene)
        {
            using (var stream = new MemoryStream(0x8000))
            {
                stream.Write(scene);
                return stream.ToArray();
            }
        }
    }
}