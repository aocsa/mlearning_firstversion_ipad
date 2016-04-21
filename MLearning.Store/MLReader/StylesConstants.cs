using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace MLReader
{
    public class StyleConstants
    {

        List<LOSlideStyle> styles = new List<LOSlideStyle>();
        public StyleConstants()
        {
            //load();
            loadnewstyles();
        }


        #region New Styles

        public List<List<LOSlideStyle>> stylesList = new List<List<LOSlideStyle>>();

        void loadnewstyles()
        {

            

            List<LOSlideStyle> greenStyle = new List<LOSlideStyle>();
            List<LOSlideStyle> redStyle = new List<LOSlideStyle>();
            List<LOSlideStyle> blueStyle = new List<LOSlideStyle>();
            List<LOSlideStyle> purpleStyle = new List<LOSlideStyle>();


            byte maxalpha = 255;
            byte midalpha = 146;


            //green colors
            Color green = Color.FromArgb(maxalpha, 112, 222, 23);
            Color green_mid_alpha = Color.FromArgb(midalpha, 112, 222, 23);
            Color light_green = Color.FromArgb(maxalpha, 202, 255, 62);


            greenStyle.Add(new LOSlideStyle { TitleColor = green, BorderColor = Colors.Black, BackgroundColor = Colors.Black, ContentColor = Colors.White });

            /* 1 */
            greenStyle.Add(new LOSlideStyle { TitleColor = green, BorderColor = green_mid_alpha, BackgroundColor = Colors.White, ContentColor = Colors.Black });
            greenStyle.Add(new LOSlideStyle { TitleColor = Colors.White, BorderColor = Colors.Black, BackgroundColor = green, ContentColor = Colors.Black });
            greenStyle.Add(new LOSlideStyle { TitleColor = light_green, BorderColor = Colors.Black, BackgroundColor = Colors.White, ContentColor = Colors.Black });
            greenStyle.Add(new LOSlideStyle { TitleColor = Colors.White, BorderColor = green, BackgroundColor = light_green, ContentColor = Colors.Black });
            /* 5 */
            greenStyle.Add(new LOSlideStyle { TitleColor = Colors.Black, BorderColor = Colors.Black, BackgroundColor = Colors.White, ContentColor = Colors.Black });
            greenStyle.Add(new LOSlideStyle { TitleColor = Colors.White, BorderColor = Colors.Black, BackgroundColor = Colors.Black, ContentColor = Colors.White });
            greenStyle.Add(new LOSlideStyle { TitleColor = green, BorderColor = green_mid_alpha, BackgroundColor = Colors.White, ContentColor = Colors.Black });

            //Add to StylesList

            stylesList.Add(greenStyle);


            //red colors
            Color red = Color.FromArgb(maxalpha, 255, 71, 69);
            Color red_mid_alpha = Color.FromArgb(midalpha, 255, 71, 69);
            Color light_red = Color.FromArgb(maxalpha, 250, 191, 57);

            redStyle.Add(new LOSlideStyle { TitleColor = red, BorderColor = Colors.Black, BackgroundColor = Colors.Black, ContentColor = Colors.White });

            /* 1 */
            redStyle.Add(new LOSlideStyle { TitleColor = red, BorderColor = red_mid_alpha, BackgroundColor = Colors.White, ContentColor = Colors.Black });
            redStyle.Add(new LOSlideStyle { TitleColor = Colors.White, BorderColor = Colors.Black, BackgroundColor = red, ContentColor = Colors.Black });
            redStyle.Add(new LOSlideStyle { TitleColor = light_red, BorderColor = Colors.Black, BackgroundColor = Colors.White, ContentColor = Colors.Black });
            redStyle.Add(new LOSlideStyle { TitleColor = Colors.White, BorderColor = red_mid_alpha, BackgroundColor = light_red, ContentColor = Colors.Black });
            /* 5 */
            redStyle.Add(new LOSlideStyle { TitleColor = Colors.Black, BorderColor = Colors.Black, BackgroundColor = Colors.White, ContentColor = Colors.Black });
            redStyle.Add(new LOSlideStyle { TitleColor = Colors.White, BorderColor = Colors.Black, BackgroundColor = Colors.Black, ContentColor = Colors.White });
            redStyle.Add(new LOSlideStyle { TitleColor = red, BorderColor = red_mid_alpha, BackgroundColor = Colors.White, ContentColor = Colors.Black });

            //Add to StylesList

            stylesList.Add(redStyle);


            //blue colors
            Color blue = Color.FromArgb(maxalpha, 92, 245, 255);
            Color blue_mid_alpha = Color.FromArgb(midalpha, 92, 245, 255);
            Color light_blue = Color.FromArgb(maxalpha, 0, 163, 151);


            blueStyle.Add(new LOSlideStyle { TitleColor = blue, BorderColor = Colors.Black, BackgroundColor = Colors.Black, ContentColor = Colors.White });

            /* 1 */
            blueStyle.Add(new LOSlideStyle { TitleColor = blue, BorderColor = blue_mid_alpha, BackgroundColor = Colors.White, ContentColor = Colors.Black });
            blueStyle.Add(new LOSlideStyle { TitleColor = Colors.White, BorderColor = Colors.Black, BackgroundColor = blue, ContentColor = Colors.Black });
            blueStyle.Add(new LOSlideStyle { TitleColor = light_blue, BorderColor = Colors.Black, BackgroundColor = Colors.White, ContentColor = Colors.Black });
            blueStyle.Add(new LOSlideStyle { TitleColor = Colors.White, BorderColor = blue, BackgroundColor = light_blue, ContentColor = Colors.Black });
            /* 5 */
            blueStyle.Add(new LOSlideStyle { TitleColor = Colors.Black, BorderColor = Colors.Black, BackgroundColor = Colors.White, ContentColor = Colors.Black });
            blueStyle.Add(new LOSlideStyle { TitleColor = Colors.White, BorderColor = Colors.Black, BackgroundColor = Colors.Black, ContentColor = Colors.White });
            blueStyle.Add(new LOSlideStyle { TitleColor = blue, BorderColor = blue_mid_alpha, BackgroundColor = Colors.White, ContentColor = Colors.Black });

            //Add to StylesList

            stylesList.Add(blueStyle);


            //purple colors
            Color purple = Color.FromArgb(maxalpha, 249, 98, 88);
            Color purple_mid_alpha = Color.FromArgb(midalpha, 249, 98, 88);
            Color light_purple = Color.FromArgb(maxalpha, 228, 42, 214);


            purpleStyle.Add(new LOSlideStyle { TitleColor = purple, BorderColor = Colors.Black, BackgroundColor = Colors.Black, ContentColor = Colors.White });

            /* 1 */
            purpleStyle.Add(new LOSlideStyle { TitleColor = purple, BorderColor = purple_mid_alpha, BackgroundColor = Colors.White, ContentColor = Colors.Black });
            purpleStyle.Add(new LOSlideStyle { TitleColor = Colors.White, BorderColor = Colors.Black, BackgroundColor = purple, ContentColor = Colors.Black });
            purpleStyle.Add(new LOSlideStyle { TitleColor = light_purple, BorderColor = Colors.Black, BackgroundColor = Colors.White, ContentColor = Colors.Black });
            purpleStyle.Add(new LOSlideStyle { TitleColor = Colors.White, BorderColor = purple, BackgroundColor = light_purple, ContentColor = Colors.Black });
            /* 5 */
            purpleStyle.Add(new LOSlideStyle { TitleColor = Colors.Black, BorderColor = Colors.Black, BackgroundColor = Colors.White, ContentColor = Colors.Black });
            purpleStyle.Add(new LOSlideStyle { TitleColor = Colors.White, BorderColor = Colors.Black, BackgroundColor = Colors.Black, ContentColor = Colors.White });
            purpleStyle.Add(new LOSlideStyle { TitleColor = purple, BorderColor = purple_mid_alpha, BackgroundColor = Colors.White, ContentColor = Colors.Black });

            //Add to StylesList

            stylesList.Add(purpleStyle);
        }

        #endregion


        #region OldStyles

        public void load ()
        { 
            styles.Add(new LOSlideStyle() { TitleColor = Colors.Yellow, ContentColor = Colors.Black, BackgroundColor = Colors.White, BorderColor = Colors.Bisque });
            styles.Add(new LOSlideStyle() { TitleColor = Colors.White, ContentColor = Colors.Black, BackgroundColor = Colors.Yellow, BorderColor = Colors.White });
            styles.Add(new LOSlideStyle() { TitleColor = Colors.Red, ContentColor = Colors.Black, BackgroundColor = Colors.White, BorderColor = Colors.Red });
            styles.Add(new LOSlideStyle() { TitleColor = Colors.White, ContentColor = Colors.Black, BackgroundColor = Colors.Red, BorderColor = Colors.Orange });
            styles.Add(new LOSlideStyle() { TitleColor = Colors.White, ContentColor = Colors.White, BackgroundColor = Colors.White, BorderColor = Colors.Bisque });
            styles.Add(new LOSlideStyle() { TitleColor = Colors.Black, ContentColor = Colors.Black, BackgroundColor = Colors.White, BorderColor = Colors.Bisque });
             
        }

          

        /// <summary>
        /// //////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>



 

        public LOSlideSource GetType0()
        {
            LOSlideSource slide = new LOSlideSource()
            {
                Type = 0,
                Style = styles[0],
                //Image = new BitmapImage(new Uri("ms-appx:///1_2.jpg")),
                Title = "PETROGLIFOS DE TORO MUERTO",
                Paragraph = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, re et dolore magna aliqua.sunt in culpa qui officia deserunt mollit anim id est laborum."
            };
            return slide;
        }

        public  LOSlideSource GetType1()
        {
            LOSlideSource slide = new LOSlideSource()
            {
                Type = 1, 
                Style =  styles[0],
                //Image = new BitmapImage(new Uri("ms-appx:///1_2.jpg")),
                Title = "PETROGLIFOS DE TORO MUERTO",
                Paragraph = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.\n\n Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. \n\n Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
            }; 
            return slide;
        }

        public  LOSlideSource GetType2()
        {
            LOSlideSource slide = new LOSlideSource()
            {
                Type = 2,
                Style  = styles[1],
                Title = "PETROGLIFOS DE TORO MUERTO",
                Paragraph = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.\n\n Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. \n\n Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
            };

            return slide;
        }

        public  LOSlideSource GetType3()
        {
            LOSlideSource slide = new LOSlideSource()
            {
                Type = 3,
                Style = styles[2],
                Title = "Fracmento del poeta loncco, Feliz Garcia Salas"
            };

            slide.Itemize = new System.Collections.ObjectModel.ObservableCollection<LOItemSource>();
            for (int i = 0; i < 12; i++)
            {
                LOItemSource item = new LOItemSource();
                if (i % 3 == 0) item.Text = "1Lorem ipsum dolor sit amet";
                if (i % 3 == 1) item.Text = "2Lorem ipsum dolor sit am  et, consectetur adipiscing elit";
                if (i % 3 == 2) item.Text = "3Lorem ipsum dolor sit amet sakd;lkasld  jsdaljada skljsadas dlkasl sd askdjaskld k alksdsalkdj daslkj consesed do eiusmod tempor incididunt ut labore et dolore magna aliqua";
                slide.Itemize.Add(item);
            }

            return slide;
        }

        public  LOSlideSource GetType1b()
        {
            LOSlideSource slide = new LOSlideSource()
            {
                Type = 1,
                Style = styles[0],
                //Image = new BitmapImage(new Uri("ms-appx:///1_2.jpg")),
                Title = "PETROGLIFOS DE TORO MUERTO",
                Paragraph = "Lorem ipsum dolor sit amet, consectetur adipiscing  dolore magna aliqua.\n\n Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
            };

            return slide;
        }

        public  LOSlideSource GetType2b()
        {
            LOSlideSource slide = new LOSlideSource()
            {
                Type = 2,
                Style = styles[2],
                Title = "PETROGLIFOS DE TORO MUERTO",
                Paragraph = "Lorem ipsum dolor sit amet, consectetur adipiscing  dolore magna aliqua.\n\n Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
            };

            return slide;
        }

        public  LOSlideSource GetType3b()
        {
            LOSlideSource slide = new LOSlideSource()
            {
                Type = 3,
                Style = styles[2],
                Title = "Fracmento del poeta loncco, Feliz Garcia Salas"
            };

            slide.Itemize = new System.Collections.ObjectModel.ObservableCollection<LOItemSource>();
            for (int i = 0; i < 5; i++)
            {
                LOItemSource item = new LOItemSource();
                if (i % 2 == 0) item.Text = "1Lorem ipsum dolor sit amet";
                if (i % 2 == 1) item.Text = "2Lorem ipsum dolor sit am  et, consectetur adipiscing elit"; 
                slide.Itemize.Add(item);
            }

            return slide;
        }

        public  LOSlideSource GetType4()
        {
            LOSlideSource slide = new LOSlideSource()
            {
                Type = 4,
                Style = styles[3],
                Title = "Fracmento del poeta loncco, Feliz Garcia Salas"
            };

            slide.Itemize = new System.Collections.ObjectModel.ObservableCollection<LOItemSource>();
            for (int i = 0; i < 3; i++)
            {
                LOItemSource item = new LOItemSource();
                item.Text = "Lorem ipsum dolor sit amet";
               // item.Image = new BitmapImage(new Uri("ms-appx:///1_2.jpg"));
                slide.Itemize.Add(item);
            }

            return slide;
        }

        public  LOSlideSource GetType5()
        {
            LOSlideSource slide = new LOSlideSource()
            {
                Type = 5,
                Style = styles[5],
                Title = "Fracmento del poeta loncco, Feliz Garcia Salas",
                Paragraph = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.\n\n Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. \n\n Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
            };

            return slide;
        }

        public  LOSlideSource GetType6()
        {
            LOSlideSource slide = new LOSlideSource()
            {
                Type = 6,
                Style = styles[4],
                Title = "PETROGLIFOS DE TORO MUERTO",
                //Image = new BitmapImage(new Uri("ms-appx:///1_2.jpg")),
                Paragraph = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.\n\n Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. \n\n Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
            };

            return slide;
        }
        #endregion
    }
}
