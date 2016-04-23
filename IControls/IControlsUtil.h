#include "pch.h"

namespace IControls
{
#ifndef UTIL_H
#define UTIL_H

	public enum class StackViewState
	{
		Open, Close
	};


	public enum class SelectionType
	{
		ItemType, StackType, None
	};

	public delegate void StackItemFullAnimationStartedEventHandler(Platform::Object ^ sender, int32 chapter, int32 section, int32 page);
	public delegate void StackItemFullAnimationCompletedEventHandler(Platform::Object ^ sender, int32 chapter, int32 section, int32 page);
	public delegate void StackItemThumbAnimationStartedEventHandler(Platform::Object ^ sender, int32 chapter, int32 section, int32 page);
	public delegate void StackItemThumbAnimationCompletedEventHandler(Platform::Object ^ sender, int32 chapter, int32 section, int32 page);	

	public delegate void IControlsComponentSelectedEventHandler(Platform::Object ^ sender, SelectionType t, int32 index);


	static float64 DeviceHeight = 900.0;
	static float64 DeviceWidth = 1600.0;
	static float64 ThumbWidth = 267.0;
	static float64 ThumbHeight = 150.0;
	static float64 FrameWidth = 305.0;
	static float64 FrameHeight = 210.0;
	static float64 ItemStackWidth = 315.0;
	static float64 ItemStackHeight = 210.0;
	static float64 StackWidth = 381.0;
	static float64 StackHeight = 335.0;
	static float64 DeltaY = (900 - 335) / 2 + 4; //translate of Y 
	static float64 ThumbScale = 6.0; // 1600.0 / 267.0 ;
	

#endif 

}