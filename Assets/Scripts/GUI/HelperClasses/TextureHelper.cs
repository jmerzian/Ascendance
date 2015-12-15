using UnityEngine;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public static class TextureHelper
{
	private static Color nothing = new Color(0,0,0,0);
	
	public static Color[] newImage(int x, int y, Color color)
	{
		Color[] colorArray = new Color[x*y];

		for(int i = 0; i < colorArray.Length; i++)
		{
			colorArray[i] = color;
		}

		return colorArray;
	}

	//helper function to copy textures
	public static void Copy(ref Texture2D output, Texture2D source)
	{
		Color[] textureData = source.GetPixels ();
		if(textureData.Length > 0)output.SetPixels (textureData);
		else Debug.Log("No color Data");
		output.Apply ();
	}
	public static void Clear (ref Texture2D texture)
	{
		Color[] pixelData = texture.GetPixels ();
		for(int i = 0; i < pixelData.Length; i++)
		{
			pixelData[i] = new Color(0,0,0,0);
		}
		texture.SetPixels (pixelData);
	}

	public static Texture2D invertMask(Texture2D mask)
	{
		Texture2D newMask = new Texture2D(mask.width,mask.height);
		Color[] newColors = mask.GetPixels ();

		for(int i = 0; i < newColors.Length; i++)
		{
			newColors[i] = new Color(0,0,0,1-newColors[i].a);
		}

		newMask.SetPixels (newColors);
		newMask.Apply ();

		return newMask;
	}

	//rotate the image in 90 increments
	public static Texture2D rotate(Texture2D texture,Vector2 rotation)
	{
		Texture2D returnTexture = new Texture2D (texture.width, texture.height);
		float angle = Mathf.Atan2 (rotation.x, rotation.y);

		//Debug.Log (angle);
		if (rotation.x == 0 || rotation.y == 0)
						angle += Mathf.PI / 4;

		if(angle < -Mathf.PI)
		{
			//Debug.Log("-270 rotation");
			for(int x = 0; x <= texture.width; x++)
			{
				for(int y = 0; y <= texture.width; y++)
				{
					returnTexture.SetPixel(x,y,texture.GetPixel(x,y));
				}
			}
		}
		else if(angle < -Mathf.PI/2)
		{
			//Debug.Log("0 rotation");
			for(int x = 0; x < texture.width; x++)
			{
				for(int y = 0; y < texture.width; y++)
				{
					returnTexture.SetPixel(x,y,texture.GetPixel(texture.height-1-y,x));
				}
			}
		}
		else if(angle < 0)
		{
			//Debug.Log("90 rotation");
			for(int x = 0; x < texture.width; x++)
			{
				for(int y = 0; y < texture.width; y++)
				{
					returnTexture.SetPixel(x,y,texture.GetPixel(texture.width-1-x,texture.height-1-y));

				}
			}
		}
		else if(angle < Mathf.PI/2)
		{
			//Debug.Log("180 rotation");
			for(int x = 0; x < texture.width; x++)
			{
				for(int y = 0; y < texture.width; y++)
				{
					returnTexture.SetPixel(x,y,texture.GetPixel(y,texture.width-1-x));

				}
			}
		}
		else if(angle < Mathf.PI)
		{
			//Debug.Log("270 rotation");
			for(int x = 0; x <= texture.width; x++)
			{
				for(int y = 0; y <= texture.width; y++)
				{
					returnTexture.SetPixel(x,y,texture.GetPixel(x,y));
				}
			}
		}
		else
		{
			//Debug.Log("360 rotation");
			for(int x = 0; x < texture.width; x++)
			{
				for(int y = 0; y < texture.width; y++)
				{
					returnTexture.SetPixel(x,y,texture.GetPixel(texture.height-1-y,x));
				}
			}
		}

		returnTexture.Apply ();

		return returnTexture;
	}

	public static Texture2D CombineTextures(Texture2D Araw,Texture2D Braw)
	{
		Texture2D returnTexture = new Texture2D(Araw.width, Araw.height);
		
		Color[] Acol = Araw.GetPixels ();
		Color[] Bcol = Braw.GetPixels ();
		Color[] returnColor = new Color[Acol.Length];
		
		for(int i = 0; i < Acol.Length; i++)
		{
			returnColor[i] = combineColors(Acol[i],Bcol[i]);
		}
		returnTexture.SetPixels (returnColor);
		returnTexture.Apply ();
		
		return returnTexture;
	}

	public static Texture2D CombineTextures(Texture2D Araw,Texture2D Braw,float scale)
	{
		Texture2D returnTexture = new Texture2D(Araw.width, Araw.height);

		Color[] Acol = Araw.GetPixels ();
		Color[] Bcol = Braw.GetPixels ();
		Color[] returnColor = new Color[Acol.Length];

		for(int i = 0; i < Acol.Length; i++)
		{
			returnColor[i] = combineColors(Acol[i],Bcol[i],scale);
		}
		returnTexture.SetPixels (returnColor);
		returnTexture.Apply ();

		return returnTexture;
	}

	public static Texture2D CombineTextures(Texture2D Araw,Texture2D Braw,Texture2D maskraw)
	{
		Texture2D returnTexture = new Texture2D(Araw.width, Araw.height);
		
		Color[] Acol = Araw.GetPixels ();
		Color[] Bcol = Braw.GetPixels ();
		Color[] mask = maskraw.GetPixels ();
		Color[] returnColor = new Color[Acol.Length];
		
		for(int i = 0; i < Acol.Length; i++)
		{
			returnColor[i] = combineColors(Acol[i],Bcol[i],mask[i].a);
		}
		returnTexture.SetPixels (returnColor);
		returnTexture.Apply ();
		
		return returnTexture;
	}

	//return a gradient of A + B starting with weighting of initOffset and creating a straight gradient to totalOffset
	//Offsets are on a scale of 0 - 1
	public static Texture2D Gradient(Texture2D Araw,Texture2D Braw,int initOffset,int nextOffset)
	{
		Texture2D returnTexture = new Texture2D(Araw.width, Araw.height);
		Texture2D maskTexture = new Texture2D (Araw.width, Araw.height);

		Color[] Acol = Araw.GetPixels ();
		Color[] Bcol = Braw.GetPixels ();
		Color[] returnColor = new Color[Acol.Length];

		for(int x = 0; x < Araw.width; x++)
		{
			Color transparancyValue = new Color(0,0,0,initOffset - (x*(initOffset-nextOffset)/Araw.width));
			Debug.Log(transparancyValue);
			for(int y = 0; y < Araw.height; y++)
			{
				maskTexture.SetPixel(x,y,transparancyValue);
			}
		}
		maskTexture.Apply ();
		Color[] mask = maskTexture.GetPixels();

		for(int i = 0; i < Acol.Length; i++)
		{
			returnColor[i] = combineColors(Acol[i],Bcol[i],mask[i].a);
		}
		returnTexture.SetPixels (returnColor);
		returnTexture.Apply ();
		
		return returnTexture;
	}


	private static Color combineColors(Color A, Color B)
	{
		Color returnColor = new Color (0,0,0,1-((1-B.a)*(1-A.a)));

		return returnColor;
	}

	private static Color combineColors(Color A, Color B,float scale)
	{
		return Color.Lerp(A,B,1-scale);
	}
}

public class TextureScale
{
	public class ThreadData
	{
		public int start;
		public int end;
		public ThreadData (int s, int e) {
			start = s;
			end = e;
		}
	}
	
	private static Color[] texColors;
	private static Color[] newColors;
	private static int w;
	private static float ratioX;
	private static float ratioY;
	private static int w2;
	private static int finishCount;
	private static Mutex mutex;
	
	public static void Point (Texture2D tex, int newWidth, int newHeight)
	{
		ThreadedScale (tex, newWidth, newHeight, false);
	}
	
	public static void Bilinear (Texture2D tex, int newWidth, int newHeight)
	{
		ThreadedScale (tex, newWidth, newHeight, true);
	}
	
	private static void ThreadedScale (Texture2D tex, int newWidth, int newHeight, bool useBilinear)
	{
		texColors = tex.GetPixels();
		newColors = new Color[newWidth * newHeight];
		if (useBilinear)
		{
			ratioX = 1.0f / ((float)newWidth / (tex.width-1));
			ratioY = 1.0f / ((float)newHeight / (tex.height-1));
		}
		else {
			ratioX = ((float)tex.width) / newWidth;
			ratioY = ((float)tex.height) / newHeight;
		}
		w = tex.width;
		w2 = newWidth;
		var cores = Mathf.Min(SystemInfo.processorCount, newHeight);
		var slice = newHeight/cores;
		
		finishCount = 0;
		if (mutex == null) {
			mutex = new Mutex(false);
		}
		if (cores > 1)
		{
			int i = 0;
			ThreadData threadData;
			for (i = 0; i < cores-1; i++) {
				threadData = new ThreadData(slice * i, slice * (i + 1));
				ParameterizedThreadStart ts = useBilinear ? new ParameterizedThreadStart(BilinearScale) : new ParameterizedThreadStart(PointScale);
				Thread thread = new Thread(ts);
				thread.Start(threadData);
			}
			threadData = new ThreadData(slice*i, newHeight);
			if (useBilinear)
			{
				BilinearScale(threadData);
			}
			else
			{
				PointScale(threadData);
			}
			while (finishCount < cores)
			{
				Thread.Sleep(1);
			}
		}
		else
		{
			ThreadData threadData = new ThreadData(0, newHeight);
			if (useBilinear)
			{
				BilinearScale(threadData);
			}
			else
			{
				PointScale(threadData);
			}
		}
		
		tex.Resize(newWidth, newHeight);
		tex.SetPixels(newColors);
		tex.Apply();
	}
	
	public static void BilinearScale (System.Object obj)
	{
		ThreadData threadData = (ThreadData) obj;
		for (var y = threadData.start; y < threadData.end; y++)
		{
			int yFloor = (int)Mathf.Floor(y * ratioY);
			var y1 = yFloor * w;
			var y2 = (yFloor+1) * w;
			var yw = y * w2;
			
			for (var x = 0; x < w2; x++) {
				int xFloor = (int)Mathf.Floor(x * ratioX);
				var xLerp = x * ratioX-xFloor;
				newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor+1], xLerp),
				                                       ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor+1], xLerp),
				                                       y*ratioY-yFloor);
			}
		}
		
		mutex.WaitOne();
		finishCount++;
		mutex.ReleaseMutex();
	}
	
	public static void PointScale (System.Object obj)
	{
		ThreadData threadData = (ThreadData) obj;
		for (var y = threadData.start; y < threadData.end; y++)
		{
			var thisY = (int)(ratioY * y) * w;
			var yw = y * w2;
			for (var x = 0; x < w2; x++) {
				newColors[yw + x] = texColors[(int)(thisY + ratioX*x)];
			}
		}
		
		mutex.WaitOne();
		finishCount++;
		mutex.ReleaseMutex();
	}
	
	private static Color ColorLerpUnclamped (Color c1, Color c2, float value)
	{
		return new Color (c1.r + (c2.r - c1.r)*value, 
		                  c1.g + (c2.g - c1.g)*value, 
		                  c1.b + (c2.b - c1.b)*value, 
		                  c1.a + (c2.a - c1.a)*value);
	}
}