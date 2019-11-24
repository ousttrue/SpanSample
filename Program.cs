using System;
using System.Text;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SpanSample
{
    static class SampleImport
    {
        const string DllName = "Sample";

        [DllImport(DllName, EntryPoint = "SAMPLE_NumberSequence")]
        public static extern void SAMPLE_NumberSequence_UseRef(ref byte bytes, int length);

        // System.Runtime.InteropServices.MarshalDirectiveException
        [DllImport(DllName, EntryPoint = "SAMPLE_NumberSequence")]
        public static extern void SAMPLE_NumberSequence_UseSpan(Span<byte> bytes, int length);

        [DllImport(DllName)]
        public static extern IntPtr SAMPLE_Create();

        [DllImport(DllName)]
        public static extern void SAMPLE_Print(IntPtr p);

        [DllImport(DllName)]
        public static extern void SAMPLE_Destroy(IntPtr p);
    }

    class Program
    {
        static string ToString(ReadOnlySpan<byte> span)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < span.Length; ++i)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(span[i]);
            }
            return sb.ToString();
        }

        static void ArraySample()
        {
            var bytes = new byte[10];
            SampleImport.SAMPLE_NumberSequence_UseRef(ref bytes[0], bytes.Length);
            Console.WriteLine(ToString(bytes));
        }

        static void SpanSample()
        {
            var bytes = new byte[10];
            var span = bytes.AsSpan();
            SampleImport.SAMPLE_NumberSequence_UseRef(ref span[0], span.Length);
            Console.WriteLine(ToString(bytes));
        }

        static void StackSample()
        {
            Span<byte> span = stackalloc byte[10];
            SampleImport.SAMPLE_NumberSequence_UseRef(ref span[0], span.Length);
            Console.WriteLine(ToString(span));
        }

        struct Dummy
        {
            public int Index;
            public Vector3 Value;
        }

        static void WritebackSample()
        {
            var p = SampleImport.SAMPLE_Create();
            {
                var d = new Dummy
                {
                    Index = 10,
                    Value = new Vector3
                    {
                        X = 1,
                        Y = 2,
                        Z = -3,
                    },
                };
                Marshal.StructureToPtr(d, p, true);
            }
            SampleImport.SAMPLE_Print(p);
            SampleImport.SAMPLE_Destroy(p);
        }

        static void WritebackStack()
        {
            var p = SampleImport.SAMPLE_Create();
            // さすがに必要
            unsafe
            {
                var span = new Span<Dummy>((void*)p, 1);
                span[0] = new Dummy
                {
                    Index = 10,
                    Value = new Vector3
                    {
                        X = 1,
                        Y = 2,
                        Z = -3,
                    },
                };
            }
            SampleImport.SAMPLE_Print(p);
            SampleImport.SAMPLE_Destroy(p);
        }

        static void WritebackCast()
        {
            var p = SampleImport.SAMPLE_Create();
            unsafe
            {
                var pd = (Dummy*)p;
                *pd = new Dummy
                {
                    Index = 10,
                    Value = new Vector3
                    {
                        X = 1,
                        Y = 2,
                        Z = -3,
                    },
                };
            }
            SampleImport.SAMPLE_Print(p);
            SampleImport.SAMPLE_Destroy(p);
        }

        static void Main(string[] args)
        {
            ArraySample();
            SpanSample();
            StackSample();

            WritebackSample();
            WritebackStack();
            WritebackCast();
        }
    }
}
