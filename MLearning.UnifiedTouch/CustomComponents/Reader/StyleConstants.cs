using System;
using System.Collections.Generic;
using UIKit;

namespace MLearning.UnifiedTouch
{
	public class StyleConstants
	{
		public StyleConstants ()
		{
			loadStyles ();
		}

		List <List<LOSlideStyle>> stylesList = new List<List<LOSlideStyle>> ();
		public List<List<LOSlideStyle>> StylesList 
		{
			get { return stylesList; }
			set { stylesList = value; }
		}

 
		void loadStyles ()
		{
			var greenStyle = new List <LOSlideStyle> ();
			var redStyle = new List <LOSlideStyle> ();
			var blueStyle = new List <LOSlideStyle> ();
			var purpleStyle = new List <LOSlideStyle> ();

			int maxAlpha = 255;
			int midAlpha = 145;


			/*********
			 * 
			 * GREEN
			 * 
			 * *******/
			UIColor green = UIColor.FromRGBA(112, 222, 23, maxAlpha);
			UIColor green_mid_alpha = UIColor.FromRGBA(112, 222, 23, midAlpha);
			UIColor light_green = UIColor.FromRGBA(202, 255, 62, maxAlpha);

			greenStyle.Add(new LOSlideStyle { TitleColor = green, BorderColor = UIColor.Blue, Background = UIColor.Black, ContentColor = UIColor.White });

			greenStyle.Add(new LOSlideStyle { TitleColor = green, BorderColor = green_mid_alpha, Background = UIColor.White, ContentColor = UIColor.Black });
			greenStyle.Add(new LOSlideStyle { TitleColor = UIColor.White, BorderColor = UIColor.Black, Background = green, ContentColor = UIColor.Black });
			greenStyle.Add(new LOSlideStyle { TitleColor = light_green, BorderColor = UIColor.Black, Background = UIColor.White, ContentColor = UIColor.Black });
			greenStyle.Add(new LOSlideStyle { TitleColor = UIColor.White, BorderColor = green, Background = light_green, ContentColor = UIColor.Black });

			greenStyle.Add(new LOSlideStyle { TitleColor = UIColor.Black, BorderColor = UIColor.Black, Background = UIColor.White, ContentColor = UIColor.Black });
			greenStyle.Add(new LOSlideStyle { TitleColor = UIColor.White, BorderColor = UIColor.Black, Background = UIColor.Black, ContentColor = UIColor.White });
			greenStyle.Add(new LOSlideStyle { TitleColor = green, BorderColor = green_mid_alpha, Background = UIColor.White, ContentColor = UIColor.Black });

			stylesList.Add (greenStyle);

			/*********
			 * 
			 * RED
			 * 
			 * *******/
			UIColor red = UIColor.FromRGBA(255, 71, 69, maxAlpha);
			UIColor red_mid_alpha = UIColor.FromRGBA(255, 71, 69, midAlpha);
			UIColor light_red = UIColor.FromRGBA(250, 191, 57, maxAlpha);

			redStyle.Add(new LOSlideStyle { TitleColor = red, BorderColor = UIColor.Black, Background = UIColor.Black, ContentColor = UIColor.White });

			redStyle.Add(new LOSlideStyle { TitleColor = red, BorderColor = red_mid_alpha, Background = UIColor.White, ContentColor = UIColor.Black });
			redStyle.Add(new LOSlideStyle { TitleColor = UIColor.White, BorderColor = UIColor.Black, Background = red, ContentColor = UIColor.Black });
			redStyle.Add(new LOSlideStyle { TitleColor = light_red, BorderColor = UIColor.Black, Background = UIColor.White, ContentColor = UIColor.Black });
			redStyle.Add(new LOSlideStyle { TitleColor = UIColor.White, BorderColor = red_mid_alpha, Background = light_red, ContentColor = UIColor.Black });

			redStyle.Add(new LOSlideStyle { TitleColor = UIColor.Black, BorderColor = UIColor.Black, Background = UIColor.White, ContentColor = UIColor.Black });
			redStyle.Add(new LOSlideStyle { TitleColor = UIColor.White, BorderColor = UIColor.Black, Background = UIColor.Black, ContentColor = UIColor.White });
			redStyle.Add(new LOSlideStyle { TitleColor = red, BorderColor = red_mid_alpha, Background = UIColor.White, ContentColor = UIColor.Black });

			stylesList.Add(redStyle);


			/*********
			* 
			* BLUE
			* 
			* *******/
			UIColor blue = UIColor.FromRGBA(92, 245, 255, maxAlpha);
			UIColor blue_mid_alpha = UIColor.FromRGBA(92, 245, 255, midAlpha);
			UIColor light_blue = UIColor.FromRGBA(0, 163, 151, maxAlpha);

			blueStyle.Add(new LOSlideStyle { TitleColor = blue, BorderColor = UIColor.Black, Background = UIColor.Black, ContentColor = UIColor.White });

			blueStyle.Add(new LOSlideStyle { TitleColor = blue, BorderColor = blue_mid_alpha, Background = UIColor.White, ContentColor = UIColor.Black });
			blueStyle.Add(new LOSlideStyle { TitleColor = UIColor.White, BorderColor = UIColor.Black, Background = blue, ContentColor = UIColor.Black });
			blueStyle.Add(new LOSlideStyle { TitleColor = light_blue, BorderColor = UIColor.Black, Background = UIColor.White, ContentColor = UIColor.Black });
			blueStyle.Add(new LOSlideStyle { TitleColor = UIColor.White, BorderColor = blue, Background = light_blue, ContentColor = UIColor.Black });

			blueStyle.Add(new LOSlideStyle { TitleColor = UIColor.Black, BorderColor = UIColor.Black, Background = UIColor.White, ContentColor = UIColor.Black });
			blueStyle.Add(new LOSlideStyle { TitleColor = UIColor.White, BorderColor = UIColor.Black, Background = UIColor.Black, ContentColor = UIColor.White });
			blueStyle.Add(new LOSlideStyle { TitleColor = blue, BorderColor = blue_mid_alpha, Background = UIColor.White, ContentColor = UIColor.Black });

			stylesList.Add(blueStyle);


			/*********
			* 
			* PURPLE
			*
			* *******/
			UIColor purple = UIColor.FromRGBA(249, 98, 88, maxAlpha);
			UIColor purple_mid_alpha = UIColor.FromRGBA(249, 98, 88, midAlpha);
			UIColor light_purple = UIColor.FromRGBA(219, 112, 147, maxAlpha);

			purpleStyle.Add(new LOSlideStyle { TitleColor = purple, BorderColor = UIColor.Black, Background = UIColor.Black, ContentColor = UIColor.White });

			purpleStyle.Add(new LOSlideStyle { TitleColor = purple, BorderColor = purple_mid_alpha, Background = UIColor.White, ContentColor = UIColor.Black });
			purpleStyle.Add(new LOSlideStyle { TitleColor = UIColor.White, BorderColor = UIColor.Black, Background = purple, ContentColor = UIColor.Black });
			purpleStyle.Add(new LOSlideStyle { TitleColor = light_purple, BorderColor = UIColor.Black, Background = UIColor.White, ContentColor = UIColor.Black });
			purpleStyle.Add(new LOSlideStyle { TitleColor = UIColor.White, BorderColor = purple, Background = light_purple, ContentColor = UIColor.Black });

			purpleStyle.Add(new LOSlideStyle { TitleColor = UIColor.Black, BorderColor = UIColor.Black, Background = UIColor.White, ContentColor = UIColor.Black });
			purpleStyle.Add(new LOSlideStyle { TitleColor = UIColor.White, BorderColor = UIColor.Black, Background = UIColor.Black, ContentColor = UIColor.White });
			purpleStyle.Add(new LOSlideStyle { TitleColor = purple, BorderColor = purple_mid_alpha, Background = UIColor.White, ContentColor = UIColor.Black });

			stylesList.Add(purpleStyle);

		}
			
	}
}

