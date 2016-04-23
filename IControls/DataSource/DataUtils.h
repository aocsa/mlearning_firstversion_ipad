
#include "pch.h"

namespace DataSource
{  
    public ref class NSFrame sealed
    {
		public: 
        property float64 Height
        {
			float64 get()  {
					return _height;
				}
			void set(float64 value) {
					_height = value;
				}
        };
        property float64 Width
        {
		float64 get() { 
					return _width;
				}
		void set(float64 value) {
					_width = value;
				}
        };
        property float64 X
        {
		float64 get() {
					return _x;
				}
		void set(float64 value) {
					_x = value;
				}
        };
        property float64 Y
        {
		float64 get() {
					return _y;
				}
		void set(float64 value) {
					_y = value;
				}
        };

	private:
		float64 _height, _width, _x, _y;
    };


#define TEXT 0
#define SIMPLE 1
#define FULL 2
#define MAP 3
#define VIDEO 4
#define VIEW360 5
#define SLIDER 5        

    public ref class NSLayer sealed
    {
	public:
		property NSFrame^ ThumbFrame
        {
            NSFrame^ get() {
						return _thumbFrame;
					}
		void set(NSFrame^ value) {
				_thumbFrame = value;
				}
        };
        property Platform::String^ ThumbPath
        {
		Platform::String^ get() {
					return _thumbPath;
				}
		void set(Platform::String^ value) {
					_thumbPath = value;
				}
        };
        property NSFrame^ Largeframe
        {
		NSFrame^ get() {
					return _largeFrame;
				}
		void set(NSFrame^ value) {
					_largeFrame = value;
				}
        };
		
		property NSFrame^ CroppedFrame
        {
		NSFrame^ get() {
					return _croppedframe;
				}
		void set(NSFrame^ value) {
					_croppedframe = value;
				}
        };
		
        property Platform::String^ LargePath
        {
			Platform::String^ get() {
					return _largePath;
				}
		void set(Platform::String^ value) {
					_largePath = value;
				}
        };

		property Platform::String^ Text
        {
			Platform::String^ get() {
					return _text;
				}
		void set(Platform::String^ value) {
					_text = value;
				}
        };

		bool  IsText () {
			if (IsSimpleLayer ())
				return false;
			return CroppedFrame == nullptr;
		}
		bool  IsFull () {
			  
			return CroppedFrame && Largeframe;
		}

		bool  IsMap () {
			  
			return Type == MAP;
		}
		
		bool  IsVideo () {
			  
			return Type == VIDEO;
		}

		bool  IsView360 () {
			  
			return Type == VIEW360;
		}
		
		bool  IsSlider () {
			  
			return Type == SLIDER;
		}

		property  int32 Type
        {
			int32   get() {
				return _type;
			}
			void set( int32  value) {
				_type = value;
			}
        };

		bool  IsSimpleLayer () {
			return Largeframe && CroppedFrame == nullptr;
		}

		private:
			int32  _type;
			NSFrame^ _thumbFrame, ^_largeFrame, ^_croppedframe ;
			Platform::String^ _thumbPath,^ _largePath, ^_text;
    };
	

	
	public ref struct NSPage sealed
    {
        public: 
		property  NSFrame^ BackgroundFrame
        {
			NSFrame^ get() {
				return _backgroundFrame;
			}
			void set( NSFrame^ value) {
				_backgroundFrame = value;
			}
        };
        property Platform::String^ BackgroundImage
        {
			Platform::String^ get() {
				return _backgroundImage;
			}
			void set(Platform::String^ value) {
				_backgroundImage = value;
			}
        };
        property Windows::Foundation::Collections::IVector< NSLayer^>^ Layers
        {
			Windows::Foundation::Collections::IVector< NSLayer^>^ get() {
				return _layers;
			}
			void set( Windows::Foundation::Collections::IVector< NSLayer^>^ value) {
				_layers = value;
			}
        };
	private:
		Platform::String^ _backgroundImage;
		NSFrame^ _backgroundFrame;
		 Windows::Foundation::Collections::IVector< NSLayer^>^ _layers;

    };

 

}