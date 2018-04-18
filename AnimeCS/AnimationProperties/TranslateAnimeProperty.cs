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
using System.Windows;
using System.Windows.Media.Animation;

namespace AnimeCS.AnimationProperties {
    public class TranslateAnimeProperty : DoubleAnimeProperty {
        public Axis axis { get; set; }
        public override IEasingFunction easing { get; set; }
        public override RepeatBehavior repeat { get; set; }
        public override IEnumerable<PropertyPath> Targets { get; set; } = new PropertyPath[] { new PropertyPath("Margin") };
        public TranslateAnimeProperty() {
            //registerAnimation = true;
        }

        public TranslateAnimeProperty(Double value) : base(value) {
            //setTargets(new PropertyPath("Margin"));
        }

        public TranslateAnimeProperty(Func<double> generator) : base(generator) { }

        public TranslateAnimeProperty(Double[] arr) : base(arr) { }


        public override Timeline getTimeLine(object el,Duration duration) {
            ThicknessAnimation timeLine;
            double startOffset, endOffset;
            var currentValue = (el as FrameworkElement).Margin;
            if (axis == Axis.X) {
                startOffset = currentValue.Left + to;
                endOffset = currentValue.Right - to;
                var end = new Thickness(startOffset,currentValue.Top,endOffset,currentValue.Bottom);
                if (useInitialValue) {
                    startOffset = currentValue.Left + from;
                    endOffset = currentValue.Right - from;
                    var start = new Thickness(startOffset,currentValue.Top,endOffset,currentValue.Bottom);
                    timeLine = new ThicknessAnimation(start,end,duration);
                } else {
                    timeLine = new ThicknessAnimation(end,duration);
                }
            } else if (axis == Axis.Y) {
                startOffset = currentValue.Top + to;
                endOffset = currentValue.Bottom - to;
                var end = new Thickness(currentValue.Left,startOffset,currentValue.Left,endOffset);
                if (useInitialValue) {
                    startOffset = currentValue.Top + from;
                    endOffset = currentValue.Bottom - from;
                    var start = new Thickness(currentValue.Left,startOffset,currentValue.Left,endOffset);
                    timeLine = new ThicknessAnimation(start,end,duration);
                } else {
                    timeLine = new ThicknessAnimation(end,duration);
                }
            } else {
                timeLine = new ThicknessAnimation(currentValue,duration);
            }
            timeLine.EasingFunction = easing;
            return timeLine;
        }

        public static implicit operator TranslateAnimeProperty(Double value) {
            return new TranslateAnimeProperty(value);
        }

        public static implicit operator TranslateAnimeProperty(Func<double> generator) {
            return new TranslateAnimeProperty(generator);
        }

        public static implicit operator TranslateAnimeProperty(Double[] arr) {
            return new TranslateAnimeProperty(arr);
        }
    }

    public enum Axis { X, Y, Z }

}
