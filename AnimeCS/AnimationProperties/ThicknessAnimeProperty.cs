/*
 * Copyright 2018 Alejandro Cardenas
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is furnished
 * to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *  
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace AnimeCS.AnimationProperties {
    public class ThicknessAnimeProperty : AnimeProperty<Thickness> {
        public override IEasingFunction easing { get; set; }
        public override RepeatBehavior repeat { get; set; }
        public ThicknessAnimeProperty() { }
        public ThicknessAnimeProperty(Thickness value) : base(value) { }

        public ThicknessAnimeProperty(TranslateAnimeProperty transX,TranslateAnimeProperty transY) {
            to = getThicknessFromArr(transX.to,transY.to);
            from = getThicknessFromArr(transX.from,transY.from);
            this.useInitialValue = transX.useInitialValue || transY.useInitialValue;
        }

        public ThicknessAnimeProperty(Func<Thickness> generator) : base(generator) { }

        public ThicknessAnimeProperty(Double margin) {
            this.to = new Thickness(margin);
        }

        public ThicknessAnimeProperty(Double[] arr) {
            this.to = getThicknessFromArr(arr);
        }

        public ThicknessAnimeProperty(String marginStr) {
            String[] valuesStr = marginStr.Split(',');
            if (valuesStr.Length > 0) {
                double[] values = new double[valuesStr.Length];
                for (int i = 0 ; i < valuesStr.Length ; i++)
                    values[i] = double.Parse(valuesStr[i].Trim());
                this.to = getThicknessFromArr(values);
            }
        }

        public override IEnumerable<PropertyPath> Targets { get; set; } = new PropertyPath[] { new PropertyPath("Margin") };

        public override Timeline getTimeLine(object el,Duration duration) {

            ThicknessAnimation timeLine;
            var currentValue = (el as FrameworkElement).Margin;
            to = new Thickness(
                to.Left + currentValue.Left,
                to.Top + currentValue.Top,
                currentValue.Right - to.Left,
                currentValue.Bottom - to.Top
            );

            if (useInitialValue) {
                from = new Thickness(
                    from.Left + currentValue.Left,
                    from.Top + currentValue.Top,
                    currentValue.Right - from.Left,
                    currentValue.Bottom - from.Top
                );
                timeLine = new ThicknessAnimation(from,to,duration);
            } else {
                timeLine = new ThicknessAnimation(to,duration);
            }
            timeLine.EasingFunction = easing;
            return timeLine;
        }

        private Thickness getThicknessFromArr(params Double[] arr) {
            Thickness margin = default(Thickness);
            if (arr.Length == 2) {
                margin = new Thickness(arr[0],arr[1],arr[0],arr[1]);
            } else if (arr.Length == 4) {
                margin = new Thickness(arr[0],arr[1],arr[2],arr[3]);
            }
            return margin;
        }

        public static implicit operator ThicknessAnimeProperty(Double value) {
            return new ThicknessAnimeProperty(value);
        }

        public static implicit operator ThicknessAnimeProperty(Func<Thickness> generator) {
            return new ThicknessAnimeProperty(generator);
        }

        public static implicit operator ThicknessAnimeProperty(Double[] arr) {
            return new ThicknessAnimeProperty(arr);
        }

        public static implicit operator ThicknessAnimeProperty(String margin) {
            return new ThicknessAnimeProperty(margin);
        }
    }
}
