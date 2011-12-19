using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

namespace avilib
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class AviWriter 
	{
		[StructLayout(LayoutKind.Sequential, Pack=1)]
			private struct AVISTREAMINFOW 
		{
			public UInt32    fccType;
			public UInt32    fccHandler;
			public UInt32    dwFlags;
			public UInt32    dwCaps;
			public UInt16    wPriority;
			public UInt16    wLanguage;
			public UInt32    dwScale;
			public UInt32    dwRate;
			public UInt32    dwStart;
			public UInt32    dwLength;
			public UInt32    dwInitialFrames;
			public UInt32    dwSuggestedBufferSize;
			public UInt32    dwQuality;
			public UInt32    dwSampleSize;
			public UInt32    rect_left;
			public UInt32    rect_top;
			public UInt32    rect_right;
			public UInt32    rect_bottom;
			public UInt32    dwEditCount;
			public UInt32    dwFormatChangeCount;
			public UInt16    szName0;
			public UInt16    szName1;
			public UInt16    szName2;
			public UInt16    szName3;
			public UInt16    szName4;
			public UInt16    szName5;
			public UInt16    szName6;
			public UInt16    szName7;
			public UInt16    szName8;
			public UInt16    szName9;
			public UInt16    szName10;
			public UInt16    szName11;
			public UInt16    szName12;
			public UInt16    szName13;
			public UInt16    szName14;
			public UInt16    szName15;
			public UInt16    szName16;
			public UInt16    szName17;
			public UInt16    szName18;
			public UInt16    szName19;
			public UInt16    szName20;
			public UInt16    szName21;
			public UInt16    szName22;
			public UInt16    szName23;
			public UInt16    szName24;
			public UInt16    szName25;
			public UInt16    szName26;
			public UInt16    szName27;
			public UInt16    szName28;
			public UInt16    szName29;
			public UInt16    szName30;
			public UInt16    szName31;
			public UInt16    szName32;
			public UInt16    szName33;
			public UInt16    szName34;
			public UInt16    szName35;
			public UInt16    szName36;
			public UInt16    szName37;
			public UInt16    szName38;
			public UInt16    szName39;
			public UInt16    szName40;
			public UInt16    szName41;
			public UInt16    szName42;
			public UInt16    szName43;
			public UInt16    szName44;
			public UInt16    szName45;
			public UInt16    szName46;
			public UInt16    szName47;
			public UInt16    szName48;
			public UInt16    szName49;
			public UInt16    szName50;
			public UInt16    szName51;
			public UInt16    szName52;
			public UInt16    szName53;
			public UInt16    szName54;
			public UInt16    szName55;
			public UInt16    szName56;
			public UInt16    szName57;
			public UInt16    szName58;
			public UInt16    szName59;
			public UInt16    szName60;
			public UInt16    szName61;
			public UInt16    szName62;
			public UInt16    szName63;
		}

		// vfw.h
		[StructLayout(LayoutKind.Sequential, Pack=1)]
			private struct AVICOMPRESSOPTIONS 
		{
			public UInt32   fccType;
			public UInt32   fccHandler;
			public UInt32   dwKeyFrameEvery;  // only used with AVICOMRPESSF_KEYFRAMES
			public UInt32   dwQuality;
			public UInt32   dwBytesPerSecond; // only used with AVICOMPRESSF_DATARATE
			public UInt32   dwFlags;
			public IntPtr   lpFormat;
			public UInt32   cbFormat;
			public IntPtr   lpParms;
			public UInt32   cbParms;
			public UInt32   dwInterleaveEvery;
		}

		[StructLayout(LayoutKind.Sequential, Pack=1)]
			public struct BITMAPINFOHEADER 
		{
			public UInt32 biSize;
			public  Int32 biWidth;
			public  Int32 biHeight;
			public  Int16 biPlanes;
			public  Int16 biBitCount;
			public UInt32 biCompression;
			public UInt32 biSizeImage;
			public  Int32 biXPelsPerMeter;
			public  Int32 biYPelsPerMeter;
			public UInt32 biClrUsed;
			public UInt32 biClrImportant;
		}

		public class AviException :  ApplicationException 
		{
			public AviException(string s) : base(s) {}
			public AviException(string s, Int32 hr) : base(s) 
			{
      
				if (hr==AVIERR_BADPARAM) 
				{
					err_msg = "AVIERR_BADPARAM";
				}
				else 
				{
					err_msg = "unknown";
				}
			}
  
			public string ErrMsg() 
			{
				return err_msg;
			}
			private const Int32 AVIERR_BADPARAM = -2147205018;
			private string err_msg;
		}

		public Bitmap Open(string fileName, UInt32 frameRate, int width, int height) 
		{
			frameRate_ = frameRate;
			width_     = (UInt32) width;
			height_    = (UInt32) height;
			bmp_       = new Bitmap(width,height,PixelFormat.Format24bppRgb);
			BitmapData bmpDat = bmp_.LockBits(new Rectangle(0,0,width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
			stride_    = (UInt32) bmpDat.Stride;
			bmp_.UnlockBits(bmpDat);
			AVIFileInit();
			int hr = AVIFileOpenW(ref pfile_, fileName, 4097 /* OF_WRITE | OF_CREATE (winbase.h) */, 0);
			if (hr != 0) 
			{
				throw new AviException("error for AVIFileOpenW");
			}

			CreateStream();
			SetOptions();

			return bmp_;
		}

		public void AddFrame() 
		{

			BitmapData bmpDat = bmp_.LockBits(
				new Rectangle(0,0,(int) width_, (int) height_),ImageLockMode.ReadOnly,PixelFormat.Format24bppRgb);

			int hr= AVIStreamWrite(psCompressed_, count_, 1, 
				bmpDat.Scan0, // pointer to data
				(Int32) (stride_  * height_), 
				0, // 16 = AVIIF_KEYFRAMe
				0,
				0); 

			if (hr != 0) 
			{
				throw new AviException("AVIStreamWrite");
			}

			bmp_.UnlockBits(bmpDat);

			count_ ++;
		}

		public void Close() 
		{
			AVIStreamRelease(ps_);
			AVIStreamRelease(psCompressed_);

			AVIFileRelease(pfile_);
			AVIFileExit();
		}

		private void CreateStream() 
		{
			AVISTREAMINFOW strhdr           = new AVISTREAMINFOW();
			strhdr.fccType               = fccType_;
			strhdr.fccHandler            = fccHandler_;
			strhdr.dwFlags               = 0;
			strhdr.dwCaps                = 0;
			strhdr.wPriority             = 0;
			strhdr.wLanguage             = 0;
			strhdr.dwScale               = 1;
			strhdr.dwRate                = frameRate_; // Frames per Second
			strhdr.dwStart               = 0;
			strhdr.dwLength              = 0;
			strhdr.dwInitialFrames       = 0;
			strhdr.dwSuggestedBufferSize = height_ * stride_;
			strhdr.dwQuality             = 0xffffffff; //-1;         // Use default
			strhdr.dwSampleSize          = 0;
			strhdr.rect_top              = 0;
			strhdr.rect_left             = 0;
			strhdr.rect_bottom           = height_;
			strhdr.rect_right            = width_;
			strhdr.dwEditCount           = 0;
			strhdr.dwFormatChangeCount   = 0;
			strhdr.szName0               = 0;
			strhdr.szName1               = 0;

			int hr = AVIFileCreateStream(pfile_, out ps_, ref strhdr);

			if (hr != 0) 
			{
				throw new AviException("AVIFileCreateStream");
			}
		}

		unsafe private void SetOptions() 
		{
			AVICOMPRESSOPTIONS opts = new AVICOMPRESSOPTIONS();
			opts.fccType           = fccType_;
			opts.fccHandler        = 0;//fccHandler_;
			opts.dwKeyFrameEvery   = 0;
			opts.dwQuality         = 0;  // 0 .. 10000
			opts.dwFlags           = 0;  // AVICOMRPESSF_KEYFRAMES = 4
			opts.dwBytesPerSecond  = 0;
			opts.lpFormat          = new IntPtr(0);
			opts.cbFormat          = 0;
			opts.lpParms           = new IntPtr(0);
			opts.cbParms           = 0;
			opts.dwInterleaveEvery = 0;

			AVICOMPRESSOPTIONS* p = &opts;
			AVICOMPRESSOPTIONS** pp = &p;

			IntPtr x       = ps_;
			IntPtr* ptr_ps = &x;

			AVISaveOptions(0,0,1,ptr_ps,pp);



			// TODO: AVISaveOptionsFree(...)
    
			int hr = AVIMakeCompressedStream(out psCompressed_, ps_, ref opts, 0);
			if (hr != 0) 
			{
				throw new AviException("AVIMakeCompressedStream");
			}

			BITMAPINFOHEADER bi    = new BITMAPINFOHEADER();
			bi.biSize         = 40;
			bi.biWidth        = (Int32) width_;
			bi.biHeight       = (Int32) height_;
			bi.biPlanes       = 1;
			bi.biBitCount     = 24;
			bi.biCompression  = 0;  // 0 = BI_RGB
			bi.biSizeImage    = stride_*height_;
			bi.biXPelsPerMeter= 0;
			bi.biYPelsPerMeter= 0;
			bi.biClrUsed      = 0;
			bi.biClrImportant = 0;

			hr = AVIStreamSetFormat(psCompressed_, 0, ref bi, 40);
			if (hr != 0) 
			{
				throw new AviException("AVIStreamSetFormat",hr);
			}
		}

		[DllImport("avifil32.dll")]
		private static extern void AVIFileInit();

		[DllImport("avifil32.dll")]
		private static extern int AVIFileOpenW(ref int ptr_pfile, [MarshalAs(UnmanagedType.LPWStr)]string fileName,int flags, int dummy);

		[DllImport("avifil32.dll")]
		private static extern int AVIFileCreateStream(int ptr_pfile, out IntPtr ptr_ptr_avi, ref AVISTREAMINFOW ptr_streaminfo); 
		[DllImport("avifil32.dll")]
		private static extern int AVIMakeCompressedStream(out IntPtr ppsCompressed, IntPtr aviStream, ref AVICOMPRESSOPTIONS ao, int dummy);

		[DllImport("avifil32.dll")]
		private static extern int AVIStreamSetFormat(IntPtr aviStream, Int32 lPos, ref BITMAPINFOHEADER lpFormat, Int32 cbFormat);

		[DllImport("avifil32.dll")]
		unsafe private static extern int AVISaveOptions(int hwnd, UInt32 flags, int nStreams, IntPtr* ptr_ptr_avi, AVICOMPRESSOPTIONS** ao);

		[DllImport("avifil32.dll")]
		private static extern int AVIStreamWrite(IntPtr aviStream, Int32 lStart, Int32 lSamples, IntPtr lpBuffer, Int32 cbBuffer, Int32 dwFlags, Int32 dummy1, Int32 dummy2);

		[DllImport("avifil32.dll")]
		private static extern int AVIStreamRelease(IntPtr aviStream);

		[DllImport("avifil32.dll")]
		private static extern int AVIFileRelease(int pfile);

		[DllImport("avifil32.dll")]
		private static extern void AVIFileExit();
  
		private int    pfile_        = 0;
		private IntPtr ps_           = new IntPtr(0);
		private IntPtr psCompressed_ = new IntPtr(0);
		private UInt32 frameRate_    = 0;
		private int    count_        = 0;
		private UInt32 width_        = 0;
		private UInt32 stride_       = 0;
		private UInt32 height_       = 0;
		private UInt32 fccType_      = 1935960438;  // vids
		private UInt32 fccHandler_   = 808810089;// IV50
		//1145656899;  // CVID
		private Bitmap bmp_;
	};
}