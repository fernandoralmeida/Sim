using System;
using System.Runtime.InteropServices;

namespace Sim.Pdf.Helpers
{
    class Methods
    {
        const uint FZ_STORE_DEFAULT = 256 << 20;
        const string DLL = "libmupdf.dll";
        // please modify the version number to match the FZ_VERSION definition in "fitz\version.h" file
        const string MuPDFVersion = "1.6";

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "fz_new_context_imp")]
        static extern IntPtr NewContext(IntPtr alloc, IntPtr locks, uint max_store, string version);

        public static IntPtr NewContext()
        {
            //return NewContext(IntPtr.Zero, IntPtr.Zero, FZ_STORE_DEFAULT, MuPDFVersion);
            return NewContext(IntPtr.Zero, IntPtr.Zero, FZ_STORE_DEFAULT, MuPDFVersion);
        }

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "fz_free_context")]
        public static extern IntPtr FreeContext(IntPtr ctx);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "fz_open_file_w", CharSet = CharSet.Unicode)]
        public static extern IntPtr OpenFile(IntPtr ctx, string fileName);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pdf_open_document_with_stream")]
        public static extern IntPtr OpenDocumentStream(IntPtr ctx, IntPtr stm);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "fz_close")]
        public static extern IntPtr CloseStream(IntPtr stm);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pdf_close_document")]
        public static extern IntPtr CloseDocument(IntPtr doc);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pdf_count_pages")]
        public static extern int CountPages(IntPtr doc);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pdf_bound_page")]
        public static extern void BoundPage(IntPtr doc, IntPtr page, ref Rectangle bound);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "fz_clear_pixmap_with_value")]
        public static extern void ClearPixmap(IntPtr ctx, IntPtr pix, int byteValue);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "fz_lookup_device_colorspace")]
        public static extern IntPtr LookupDeviceColorSpace(IntPtr ctx, string colorspace);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "fz_free_device")]
        public static extern void FreeDevice(IntPtr dev);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pdf_free_page")]
        public static extern void FreePage(IntPtr doc, IntPtr page);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pdf_load_page")]
        public static extern IntPtr LoadPage(IntPtr doc, int pageNumber);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "fz_new_draw_device")]
        public static extern IntPtr NewDrawDevice(IntPtr ctx, IntPtr pix);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "fz_new_pixmap")]
        public static extern IntPtr NewPixmap(IntPtr ctx, IntPtr colorspace, int width, int height);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "pdf_run_page")]
        public static extern void RunPage(IntPtr doc, IntPtr page, IntPtr dev, ref Matrix transform, IntPtr cookie);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "fz_drop_pixmap")]
        public static extern void DropPixmap(IntPtr ctx, IntPtr pix);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "fz_pixmap_samples")]
        public static extern IntPtr GetSamples(IntPtr ctx, IntPtr pix);
    }
}
