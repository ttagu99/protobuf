﻿using System;
using System.IO;
using Google.Protobuf.TestProtos;
using NUnit.Framework;

namespace Google.Protobuf
{
    /// <summary>
    /// Tests around the generated TestAllTypes message.
    /// </summary>
    public class GeneratedMessageTest
    {
        [Test]
        public void EmptyMessageFieldDistinctFromMissingMessageField()
        {
            // This demonstrates what we're really interested in...
            var message1 = new TestAllTypes { SingleForeignMessage = new ForeignMessage() };
            var message2 = new TestAllTypes(); // SingleForeignMessage is null
            EqualityTester.AssertInequality(message1, message2);
        }

        [Test]
        public void DefaultValues()
        {
            // Single fields
            var message = new TestAllTypes();
            Assert.AreEqual(false, message.SingleBool);
            Assert.AreEqual(ByteString.Empty, message.SingleBytes);
            Assert.AreEqual(0.0, message.SingleDouble);
            Assert.AreEqual(0, message.SingleFixed32);
            Assert.AreEqual(0L, message.SingleFixed64);
            Assert.AreEqual(0.0f, message.SingleFloat);
            Assert.AreEqual(ForeignEnum.FOREIGN_UNSPECIFIED, message.SingleForeignEnum);
            Assert.IsNull(message.SingleForeignMessage);
            Assert.AreEqual(ImportEnum.IMPORT_ENUM_UNSPECIFIED, message.SingleImportEnum);
            Assert.IsNull(message.SingleImportMessage);
            Assert.AreEqual(0, message.SingleInt32);
            Assert.AreEqual(0L, message.SingleInt64);
            Assert.AreEqual(TestAllTypes.Types.NestedEnum.NESTED_ENUM_UNSPECIFIED, message.SingleNestedEnum);
            Assert.IsNull(message.SingleNestedMessage);
            Assert.IsNull(message.SinglePublicImportMessage);
            Assert.AreEqual(0, message.SingleSfixed32);
            Assert.AreEqual(0L, message.SingleSfixed64);
            Assert.AreEqual(0, message.SingleSint32);
            Assert.AreEqual(0L, message.SingleSint64);
            Assert.AreEqual("", message.SingleString);
            Assert.AreEqual(0U, message.SingleUint32);
            Assert.AreEqual(0UL, message.SingleUint64);

            // Repeated fields
            Assert.AreEqual(0, message.RepeatedBool.Count);
            Assert.AreEqual(0, message.RepeatedBytes.Count);
            Assert.AreEqual(0, message.RepeatedDouble.Count);
            Assert.AreEqual(0, message.RepeatedFixed32.Count);
            Assert.AreEqual(0, message.RepeatedFixed64.Count);
            Assert.AreEqual(0, message.RepeatedFloat.Count);
            Assert.AreEqual(0, message.RepeatedForeignEnum.Count);
            Assert.AreEqual(0, message.RepeatedForeignMessage.Count);
            Assert.AreEqual(0, message.RepeatedImportEnum.Count);
            Assert.AreEqual(0, message.RepeatedImportMessage.Count);
            Assert.AreEqual(0, message.RepeatedNestedEnum.Count);
            Assert.AreEqual(0, message.RepeatedNestedMessage.Count);
            Assert.AreEqual(0, message.RepeatedPublicImportMessage.Count);
            Assert.AreEqual(0, message.RepeatedSfixed32.Count);
            Assert.AreEqual(0, message.RepeatedSfixed64.Count);
            Assert.AreEqual(0, message.RepeatedSint32.Count);
            Assert.AreEqual(0, message.RepeatedSint64.Count);
            Assert.AreEqual(0, message.RepeatedString.Count);
            Assert.AreEqual(0, message.RepeatedUint32.Count);
            Assert.AreEqual(0, message.RepeatedUint64.Count);

            // Oneof fields
            Assert.AreEqual(TestAllTypes.OneofFieldOneofCase.None, message.OneofFieldCase);
            Assert.AreEqual(0, message.OneofUint32);
            Assert.AreEqual("", message.OneofString);
            Assert.AreEqual(ByteString.Empty, message.OneofBytes);
            Assert.IsNull(message.OneofNestedMessage);
        }

        [Test]
        public void RoundTrip_Empty()
        {
            var message = new TestAllTypes();
            // Without setting any values, there's nothing to write.
            byte[] bytes = message.ToByteArray();
            Assert.AreEqual(0, bytes.Length);
            TestAllTypes parsed = TestAllTypes.Parser.ParseFrom(bytes);
            Assert.AreEqual(message, parsed);
        }

        [Test]
        public void RoundTrip_SingleValues()
        {
            var message = new TestAllTypes
            {
                SingleBool = true,
                SingleBytes = ByteString.CopyFrom(1, 2, 3, 4),
                SingleDouble = 23.5,
                SingleFixed32 = 23,
                SingleFixed64 = 1234567890123,
                SingleFloat = 12.25f,
                SingleForeignEnum = ForeignEnum.FOREIGN_BAR,
                SingleForeignMessage = new ForeignMessage { C = 10 },
                SingleImportEnum = ImportEnum.IMPORT_BAZ,
                SingleImportMessage = new ImportMessage { D = 20 },
                SingleInt32 = 100,
                SingleInt64 = 3210987654321,
                SingleNestedEnum = TestAllTypes.Types.NestedEnum.FOO,
                SingleNestedMessage = new TestAllTypes.Types.NestedMessage { Bb = 35 },
                SinglePublicImportMessage = new PublicImportMessage { E = 54 },
                SingleSfixed32 = -123,
                SingleSfixed64 = -12345678901234,
                SingleSint32 = -456,
                SingleSint64 = -12345678901235,
                SingleString = "test",
                SingleUint32 = uint.MaxValue,
                SingleUint64 = ulong.MaxValue
            };

            byte[] bytes = message.ToByteArray();
            TestAllTypes parsed = TestAllTypes.Parser.ParseFrom(bytes);
            Assert.AreEqual(message, parsed);
        }

        [Test]
        public void RoundTrip_RepeatedValues()
        {
            var message = new TestAllTypes
            {
                RepeatedBool = { true, false },
                RepeatedBytes = { ByteString.CopyFrom(1, 2, 3, 4), ByteString.CopyFrom(5, 6) },
                RepeatedDouble = { -12.25, 23.5 },
                RepeatedFixed32 = { uint.MaxValue, 23 },
                RepeatedFixed64 = { ulong.MaxValue, 1234567890123 },
                RepeatedFloat = { 100f, 12.25f },
                RepeatedForeignEnum = { ForeignEnum.FOREIGN_FOO, ForeignEnum.FOREIGN_BAR },
                RepeatedForeignMessage = { new ForeignMessage(), new ForeignMessage { C = 10 } },
                RepeatedImportEnum = { ImportEnum.IMPORT_BAZ, ImportEnum.IMPORT_ENUM_UNSPECIFIED },
                RepeatedImportMessage = { new ImportMessage { D = 20 }, new ImportMessage { D = 25 } },
                RepeatedInt32 = { 100, 200 },
                RepeatedInt64 = { 3210987654321, long.MaxValue },
                RepeatedNestedEnum = { TestAllTypes.Types.NestedEnum.FOO, TestAllTypes.Types.NestedEnum.NEG },
                RepeatedNestedMessage = { new TestAllTypes.Types.NestedMessage { Bb = 35 }, new TestAllTypes.Types.NestedMessage { Bb = 10 } },
                RepeatedPublicImportMessage = { new PublicImportMessage { E = 54 }, new PublicImportMessage { E = -1 } },
                RepeatedSfixed32 = { -123, 123 },
                RepeatedSfixed64 = { -12345678901234, 12345678901234 },
                RepeatedSint32 = { -456, 100 },
                RepeatedSint64 = { -12345678901235, 123 },
                RepeatedString = { "foo", "bar" },
                RepeatedUint32 = { uint.MaxValue, uint.MinValue },
                RepeatedUint64 = { ulong.MaxValue, uint.MinValue }
            };

            byte[] bytes = message.ToByteArray();
            TestAllTypes parsed = TestAllTypes.Parser.ParseFrom(bytes);
            Assert.AreEqual(message, parsed);
        }

        // Note that not every map within map_unittest_proto3 is used. They all go through very
        // similar code paths. The fact that all maps are present is validation that we have codecs
        // for every type.
        [Test]
        public void RoundTrip_Maps()
        {
            var message = new TestMap
            {
                MapBoolBool = {
                    { false, true },
                    { true, false }
                },
                MapInt32Bytes = {
                    { 5, ByteString.CopyFrom(6, 7, 8) },
                    { 25, ByteString.CopyFrom(1, 2, 3, 4, 5) },
                    { 10, ByteString.Empty }
                },
                MapInt32ForeignMessage = {
                    { 0, new ForeignMessage { C = 10 } },
                    { 5, null },
                },
                MapInt32Enum = {
                    { 1, MapEnum.MAP_ENUM_BAR },
                    { 2000, MapEnum.MAP_ENUM_FOO }
                }
            };

            byte[] bytes = message.ToByteArray();
            TestMap parsed = TestMap.Parser.ParseFrom(bytes);
            Assert.AreEqual(message, parsed);
        }

        [Test]
        public void MapWithEmptyEntry()
        {
            var message = new TestMap
            {
                MapInt32Bytes = { { 0, ByteString.Empty } }
            };

            byte[] bytes = message.ToByteArray();
            Assert.AreEqual(2, bytes.Length); // Tag for field entry (1 byte), length of entry (0; 1 byte)

            var parsed = TestMap.Parser.ParseFrom(bytes);
            Assert.AreEqual(1, parsed.MapInt32Bytes.Count);
            Assert.AreEqual(ByteString.Empty, parsed.MapInt32Bytes[0]);
        }
        
        [Test]
        public void MapWithOnlyValue()
        {
            // Hand-craft the stream to contain a single entry with just a value.
            var memoryStream = new MemoryStream();
            var output = CodedOutputStream.CreateInstance(memoryStream);
            output.WriteTag(TestMap.MapInt32ForeignMessageFieldNumber, WireFormat.WireType.LengthDelimited);
            var nestedMessage = new ForeignMessage { C = 20 };
            // Size of the entry (tag, size written by WriteMessage, data written by WriteMessage)
            output.WriteRawVarint32((uint)(nestedMessage.CalculateSize() + 3));
            output.WriteTag(2, WireFormat.WireType.LengthDelimited);
            output.WriteMessage(nestedMessage);
            output.Flush();

            var parsed = TestMap.Parser.ParseFrom(memoryStream.ToArray());
            Assert.AreEqual(nestedMessage, parsed.MapInt32ForeignMessage[0]);
        }

        [Test]
        public void MapIgnoresExtraFieldsWithinEntryMessages()
        {
            // Hand-craft the stream to contain a single entry with three fields
            var memoryStream = new MemoryStream();
            var output = CodedOutputStream.CreateInstance(memoryStream);

            output.WriteTag(TestMap.MapInt32Int32FieldNumber, WireFormat.WireType.LengthDelimited);

            var key = 10; // Field 1 
            var value = 20; // Field 2
            var extra = 30; // Field 3

            // Each field can be represented in a single byte, with a single byte tag.
            // Total message size: 6 bytes.
            output.WriteRawVarint32(6);
            output.WriteTag(1, WireFormat.WireType.Varint);
            output.WriteInt32(key);
            output.WriteTag(2, WireFormat.WireType.Varint);
            output.WriteInt32(value);
            output.WriteTag(3, WireFormat.WireType.Varint);
            output.WriteInt32(extra);
            output.Flush();

            var parsed = TestMap.Parser.ParseFrom(memoryStream.ToArray());
            Assert.AreEqual(value, parsed.MapInt32Int32[key]);
        }

        [Test]
        public void MapFieldOrderIsIrrelevant()
        {
            var memoryStream = new MemoryStream();
            var output = CodedOutputStream.CreateInstance(memoryStream);

            output.WriteTag(TestMap.MapInt32Int32FieldNumber, WireFormat.WireType.LengthDelimited);

            var key = 10;
            var value = 20;

            // Each field can be represented in a single byte, with a single byte tag.
            // Total message size: 4 bytes.
            output.WriteRawVarint32(4);
            output.WriteTag(2, WireFormat.WireType.Varint);
            output.WriteInt32(value);
            output.WriteTag(1, WireFormat.WireType.Varint);
            output.WriteInt32(key);
            output.Flush();

            var parsed = TestMap.Parser.ParseFrom(memoryStream.ToArray());
            Assert.AreEqual(value, parsed.MapInt32Int32[key]);
        }

        [Test]
        public void MapNonContiguousEntries()
        {
            var memoryStream = new MemoryStream();
            var output = CodedOutputStream.CreateInstance(memoryStream);

            // Message structure:
            // Entry for MapInt32Int32
            // Entry for MapStringString
            // Entry for MapInt32Int32

            // First entry
            var key1 = 10;
            var value1 = 20;
            output.WriteTag(TestMap.MapInt32Int32FieldNumber, WireFormat.WireType.LengthDelimited);
            output.WriteRawVarint32(4);
            output.WriteTag(1, WireFormat.WireType.Varint);
            output.WriteInt32(key1);
            output.WriteTag(2, WireFormat.WireType.Varint);
            output.WriteInt32(value1);

            // Second entry
            var key2 = "a";
            var value2 = "b";
            output.WriteTag(TestMap.MapStringStringFieldNumber, WireFormat.WireType.LengthDelimited);
            output.WriteRawVarint32(6); // 3 bytes per entry: tag, size, character
            output.WriteTag(1, WireFormat.WireType.LengthDelimited);
            output.WriteString(key2);
            output.WriteTag(2, WireFormat.WireType.LengthDelimited);
            output.WriteString(value2);

            // Third entry
            var key3 = 15;
            var value3 = 25;
            output.WriteTag(TestMap.MapInt32Int32FieldNumber, WireFormat.WireType.LengthDelimited);
            output.WriteRawVarint32(4);
            output.WriteTag(1, WireFormat.WireType.Varint);
            output.WriteInt32(key3);
            output.WriteTag(2, WireFormat.WireType.Varint);
            output.WriteInt32(value3);

            output.Flush();
            var parsed = TestMap.Parser.ParseFrom(memoryStream.ToArray());
            var expected = new TestMap
            {
                MapInt32Int32 = { { key1, value1 }, { key3, value3 } },
                MapStringString = { { key2, value2 } }
            };
            Assert.AreEqual(expected, parsed);
        }

        [Test]
        public void DuplicateKeys_LastEntryWins()
        {
            var memoryStream = new MemoryStream();
            var output = CodedOutputStream.CreateInstance(memoryStream);

            var key = 10;
            var value1 = 20;
            var value2 = 30;

            // First entry
            output.WriteTag(TestMap.MapInt32Int32FieldNumber, WireFormat.WireType.LengthDelimited);
            output.WriteRawVarint32(4);
            output.WriteTag(1, WireFormat.WireType.Varint);
            output.WriteInt32(key);
            output.WriteTag(2, WireFormat.WireType.Varint);
            output.WriteInt32(value1);

            // Second entry - same key, different value
            output.WriteTag(TestMap.MapInt32Int32FieldNumber, WireFormat.WireType.LengthDelimited);
            output.WriteRawVarint32(4);
            output.WriteTag(1, WireFormat.WireType.Varint);
            output.WriteInt32(key);
            output.WriteTag(2, WireFormat.WireType.Varint);
            output.WriteInt32(value2);
            output.Flush();

            var parsed = TestMap.Parser.ParseFrom(memoryStream.ToArray());
            Assert.AreEqual(value2, parsed.MapInt32Int32[key]);
        }

        [Test]
        public void CloneSingleNonMessageValues()
        {
            var original = new TestAllTypes
            {
                SingleBool = true,
                SingleBytes = ByteString.CopyFrom(1, 2, 3, 4),
                SingleDouble = 23.5,
                SingleFixed32 = 23,
                SingleFixed64 = 1234567890123,
                SingleFloat = 12.25f,
                SingleInt32 = 100,
                SingleInt64 = 3210987654321,
                SingleNestedEnum = TestAllTypes.Types.NestedEnum.FOO,
                SingleSfixed32 = -123,
                SingleSfixed64 = -12345678901234,
                SingleSint32 = -456,
                SingleSint64 = -12345678901235,
                SingleString = "test",
                SingleUint32 = uint.MaxValue,
                SingleUint64 = ulong.MaxValue
            };
            var clone = original.Clone();
            Assert.AreNotSame(original, clone);
            Assert.AreEqual(original, clone);
            // Just as a single example
            clone.SingleInt32 = 150;
            Assert.AreNotEqual(original, clone);
        }

        [Test]
        public void CloneRepeatedNonMessageValues()
        {
            var original = new TestAllTypes
            {
                RepeatedBool = { true, false },
                RepeatedBytes = { ByteString.CopyFrom(1, 2, 3, 4), ByteString.CopyFrom(5, 6) },
                RepeatedDouble = { -12.25, 23.5 },
                RepeatedFixed32 = { uint.MaxValue, 23 },
                RepeatedFixed64 = { ulong.MaxValue, 1234567890123 },
                RepeatedFloat = { 100f, 12.25f },
                RepeatedInt32 = { 100, 200 },
                RepeatedInt64 = { 3210987654321, long.MaxValue },
                RepeatedNestedEnum = { TestAllTypes.Types.NestedEnum.FOO, TestAllTypes.Types.NestedEnum.NEG },
                RepeatedSfixed32 = { -123, 123 },
                RepeatedSfixed64 = { -12345678901234, 12345678901234 },
                RepeatedSint32 = { -456, 100 },
                RepeatedSint64 = { -12345678901235, 123 },
                RepeatedString = { "foo", "bar" },
                RepeatedUint32 = { uint.MaxValue, uint.MinValue },
                RepeatedUint64 = { ulong.MaxValue, uint.MinValue }
            };

            var clone = original.Clone();
            Assert.AreNotSame(original, clone);
            Assert.AreEqual(original, clone);
            // Just as a single example
            clone.RepeatedDouble.Add(25.5);
            Assert.AreNotEqual(original, clone);
        }

        [Test]
        public void CloneSingleMessageField()
        {
            var original = new TestAllTypes
            {
                SingleNestedMessage = new TestAllTypes.Types.NestedMessage { Bb = 20 }
            };

            var clone = original.Clone();
            Assert.AreNotSame(original, clone);
            Assert.AreNotSame(original.SingleNestedMessage, clone.SingleNestedMessage);
            Assert.AreEqual(original, clone);

            clone.SingleNestedMessage.Bb = 30;
            Assert.AreNotEqual(original, clone);
        }

        [Test]
        public void CloneRepeatedMessageField()
        {
            var original = new TestAllTypes
            {
                RepeatedNestedMessage = { new TestAllTypes.Types.NestedMessage { Bb = 20 } }
            };

            var clone = original.Clone();
            Assert.AreNotSame(original, clone);
            Assert.AreNotSame(original.RepeatedNestedMessage, clone.RepeatedNestedMessage);
            Assert.AreNotSame(original.RepeatedNestedMessage[0], clone.RepeatedNestedMessage[0]);
            Assert.AreEqual(original, clone);

            clone.RepeatedNestedMessage[0].Bb = 30;
            Assert.AreNotEqual(original, clone);
        }

        [Test]
        public void CloneOneofField()
        {
            var original = new TestAllTypes
            {
                OneofNestedMessage = new TestAllTypes.Types.NestedMessage { Bb = 20 }
            };

            var clone = original.Clone();
            Assert.AreNotSame(original, clone);
            Assert.AreEqual(original, clone);

            // We should have cloned the message
            original.OneofNestedMessage.Bb = 30;
            Assert.AreNotEqual(original, clone);
        }

        [Test]
        public void Freeze()
        {
            var frozen = new TestAllTypes();
            frozen.Freeze();
            Assert.IsTrue(frozen.IsFrozen);

            Assert.Throws<InvalidOperationException>(() => frozen.ClearOneofField());
            Assert.Throws<InvalidOperationException>(() => frozen.SingleInt32 = 0);
            Assert.Throws<InvalidOperationException>(() => frozen.SingleNestedMessage = null);
            Assert.Throws<InvalidOperationException>(() => frozen.SingleNestedEnum = 0);
            Assert.Throws<InvalidOperationException>(() => frozen.OneofString = null);
            Assert.Throws<InvalidOperationException>(() => frozen.OneofUint32 = 0U);
            Assert.Throws<InvalidOperationException>(() => frozen.RepeatedDouble.Add(0.0));
            Assert.Throws<InvalidOperationException>(() => frozen.RepeatedNestedMessage.Add(new TestAllTypes.Types.NestedMessage()));
        }
    }
}
