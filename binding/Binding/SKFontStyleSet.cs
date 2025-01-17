﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace SkiaSharp
{
	public class SKFontStyleSet : SKObject, ISKReferenceCounted, IEnumerable<SKFontStyle>, IReadOnlyCollection<SKFontStyle>, IReadOnlyList<SKFontStyle>
	{
		internal SKFontStyleSet (IntPtr handle, bool owns)
			: base (handle, owns)
		{
		}

		public SKFontStyleSet ()
			: this (SkiaApi.sk_fontstyleset_create_empty (), true)
		{
		}

		protected override void Dispose (bool disposing) =>
			base.Dispose (disposing);

		public int Count => SkiaApi.sk_fontstyleset_get_count (Handle);

		public SKFontStyle this[int index] => GetStyle (index);

		public string GetStyleName (int index)
		{
			using var str = new SKString ();
			SkiaApi.sk_fontstyleset_get_style (Handle, index, IntPtr.Zero, str.Handle);
			GC.KeepAlive (this);
			return (string)str;
		}

		public SKTypeface CreateTypeface (int index)
		{
			if (index < 0 || index >= Count)
				throw new ArgumentOutOfRangeException ($"Index was out of range. Must be non-negative and less than the size of the set.", nameof (index));

			var tf = SKTypeface.GetObject (SkiaApi.sk_fontstyleset_create_typeface (Handle, index));
			tf?.PreventPublicDisposal ();
			GC.KeepAlive (this);
			return tf;
		}

		public SKTypeface CreateTypeface (SKFontStyle style)
		{
			if (style == null)
				throw new ArgumentNullException (nameof (style));

			var tf = SKTypeface.GetObject (SkiaApi.sk_fontstyleset_match_style (Handle, style.Handle));
			tf?.PreventPublicDisposal ();
			GC.KeepAlive (this);
			return tf;
		}

		public IEnumerator<SKFontStyle> GetEnumerator () => GetStyles ().GetEnumerator ();

		IEnumerator IEnumerable.GetEnumerator () => GetStyles ().GetEnumerator ();

		private IEnumerable<SKFontStyle> GetStyles ()
		{
			var count = Count;
			for (var i = 0; i < count; i++) {
				yield return GetStyle (i);
			}
		}

		private SKFontStyle GetStyle (int index)
		{
			var fontStyle = new SKFontStyle ();
			SkiaApi.sk_fontstyleset_get_style (Handle, index, fontStyle.Handle, IntPtr.Zero);
			return fontStyle;
		}

		internal static SKFontStyleSet GetObject (IntPtr handle) =>
			GetOrAddObject (handle, (h, o) => new SKFontStyleSet (h, o));
	}
}
