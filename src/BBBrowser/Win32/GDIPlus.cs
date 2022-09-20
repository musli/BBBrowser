using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace Win32
{
    public abstract class GDIPlus
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct StartupInput
        {
            private int GdiplusVersion;

            private readonly IntPtr DebugEventCallback;

            private bool SuppressBackgroundThread;

            private bool SuppressExternalCodecs;

            public static StartupInput GetDefault()
            {
                var result = new StartupInput
                {
                    GdiplusVersion = 1,
                    SuppressBackgroundThread = false,
                    SuppressExternalCodecs = false
                };
                return result;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public readonly struct StartupOutput
        {
            private readonly IntPtr hook;

            private readonly IntPtr unhook;
        }

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GdipImageGetFrameDimensionsCount(HandleRef image, out int count);
        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GdipImageGetFrameDimensionsList(HandleRef image, IntPtr buffer, int count);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GdipImageGetFrameCount(HandleRef image, ref Guid dimensionId, int[] count);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GdipGetPropertyItemSize(HandleRef image, int propid, out int size);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GdipGetPropertyItem(HandleRef image, int propid, int size, IntPtr buffer);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.Machine)]
        public static extern int GdipCreateHBITMAPFromBitmap(HandleRef nativeBitmap, out IntPtr hbitmap, int argbBackground);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GdipImageSelectActiveFrame(HandleRef image, ref Guid dimensionId, int frameIndex);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.Machine)]
        public static extern int GdipCreateBitmapFromFile(string filename, out IntPtr bitmap);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GdipImageForceValidation(HandleRef image);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, EntryPoint = "GdipDisposeImage", CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int IntGdipDisposeImage(HandleRef image);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.Process)]
        public static extern int GdiplusStartup(out IntPtr token, ref StartupInput input, out StartupOutput output);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GdipGetImageRawFormat(HandleRef image, ref Guid format);


        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.Machine)]
        public static extern int GdipCreateBitmapFromStream(IStream stream, out IntPtr bitmap);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.Machine)]
        public static extern int GdipCreateBitmapFromHBITMAP(HandleRef hbitmap, HandleRef hpalette, out IntPtr bitmap);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GdipGetImageEncodersSize(out int numEncoders, out int size);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GdipGetImageDecodersSize(out int numDecoders, out int size);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GdipGetImageDecoders(int numDecoders, int size, IntPtr decoders);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GdipGetImageEncoders(int numEncoders, int size, IntPtr encoders);

        [DllImport("gdiplus", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int GdipSaveImageToStream(HandleRef image, IStream stream, ref Guid classId, HandleRef encoderParams);

      
        public static void PtrToStructure(IntPtr lparam, object data) => Marshal.PtrToStructure(lparam, data);

    }
    [ComImport, Guid("0000000C-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IStream
    {
        int Read([In] IntPtr buf, [In] int len);

        int Write([In] IntPtr buf, [In] int len);

        [return: MarshalAs(UnmanagedType.I8)]
        long Seek([In, MarshalAs(UnmanagedType.I8)] long dlibMove, [In] int dwOrigin);

        void SetSize([In, MarshalAs(UnmanagedType.I8)] long libNewSize);

        [return: MarshalAs(UnmanagedType.I8)]
        long CopyTo([In, MarshalAs(UnmanagedType.Interface)] IStream pstm, [In, MarshalAs(UnmanagedType.I8)] long cb, [Out, MarshalAs(UnmanagedType.LPArray)] long[] pcbRead);

        void Commit([In] int grfCommitFlags);

        void Revert();

        void LockRegion([In, MarshalAs(UnmanagedType.I8)] long libOffset, [In, MarshalAs(UnmanagedType.I8)] long cb, [In] int dwLockType);

        void UnlockRegion([In, MarshalAs(UnmanagedType.I8)] long libOffset, [In, MarshalAs(UnmanagedType.I8)] long cb, [In] int dwLockType);

        void Stat([In] IntPtr pStatstg, [In] int grfStatFlag);

        [return: MarshalAs(UnmanagedType.Interface)]
        IStream Clone();
    }
}
