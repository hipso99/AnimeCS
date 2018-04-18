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
    public class DoubleAnimeProperty : AnimeProperty<Double> {
        public DoubleAnimeProperty() { }

        public DoubleAnimeProperty(Double value) : base(value) { }

        public DoubleAnimeProperty(Func<Double> generator) : base(generator) { }

        public DoubleAnimeProperty(Double[] arr) : base(arr) { }

        public override RepeatBehavior repeat { get; set; }
        public override IEasingFunction easing { get; set; }

        public override IEnumerable<PropertyPath> Targets { get; set; }


        public override Timeline getTimeLine(object el,Duration duration) {
            DoubleAnimation timeLine;
            if (useInitialValue)
                timeLine = new DoubleAnimation(from,to,duration);
            else
                timeLine = new DoubleAnimation(to,duration);

            timeLine.EasingFunction = easing;
            return timeLine;
        }

        public static implicit operator DoubleAnimeProperty(Double value) {
            return new DoubleAnimeProperty(value);
        }

        public static implicit operator DoubleAnimeProperty(Func<Double> generator) {
            return new DoubleAnimeProperty(generator);
        }

        public static implicit operator DoubleAnimeProperty(Double[] arr) {
            return new DoubleAnimeProperty(arr);
        }
    }
}
