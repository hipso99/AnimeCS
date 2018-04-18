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
    public class AnimeProperties {
        private Dictionary<String,object> parameters = new Dictionary<string,object>();
        private Dictionary<String,IAnimeProperty> animeProperties = new Dictionary<string,IAnimeProperty>();


        public FrameworkElement target { get; set; }

        public FrameworkElement[] targets { get; set; }

        public RepeatBehavior repeat { get; set; }

        public int time { get; set; } = Anime.TIME;

        public bool loop { get; set; } = false;

        public IEasingFunction easing { get; set; }

        public int delay { get; set; }

        public DoubleAnimeProperty height {
            get {
                if (animeProperties.ContainsKey("Height"))
                    return (DoubleAnimeProperty)animeProperties["Height"];
                return 0;
            }
            set {
                if (value != null) {
                    value.setTargets(new PropertyPath("Height"));
                }
                animeProperties["Height"] = value;
            }
        }

        public DoubleAnimeProperty width {
            get {
                if (animeProperties.ContainsKey("Width"))
                    return (DoubleAnimeProperty)animeProperties["Width"];
                return 0;
            }
            set {
                if (value != null) {
                    value.setTargets(new PropertyPath("Width"));
                }
                animeProperties["Width"] = value;
            }
        }

        //public DoubleAnimeProperty scale {
        //    get {
        //        if (animeProperties.ContainsKey("scale"))
        //            return (DoubleAnimeProperty)animeProperties["scale"];
        //        return 0;
        //    }
        //    set {
        //        if (value != null) {
        //            value.setTargets(new PropertyPath("Width"));
        //        }
        //        animeProperties["scale"] = value;
        //    }
        //}

        public TranslateAnimeProperty translateX {
            get {
                if (animeProperties.ContainsKey("translateX"))
                    return (TranslateAnimeProperty)animeProperties["translateX"];
                return 0;
            }
            set {
                if (value != null) {
                    value.axis = Axis.X;
                }
                animeProperties["translateX"] = value;
            }
        }

        public TranslateAnimeProperty translateY {
            get {
                if (animeProperties.ContainsKey("translateY"))
                    return (TranslateAnimeProperty)animeProperties["translateY"];
                return 0;
            }
            set {
                if (value != null) {
                    value.axis = Axis.Y;
                }
                animeProperties["translateY"] = value;
            }
        }

        public ThicknessAnimeProperty margin { get; set; }

        public double opacity {
            get {
                if (parameters.ContainsKey("Opacity"))
                    return (double)parameters["Opacity"];
                return 0;
            }
            set {
                parameters["Opacity"] = value;
            }
        }

        public List<Tuple<string,object>> getProperties() {
            List<Tuple<string,object>> par = new List<Tuple<string,object>>();
            foreach (var el in parameters) {
                par.Add(new Tuple<string,object>(el.Key,el.Value));
            }
            return par;
        }

        public IEnumerable<IAnimeProperty> getAnimeProperties() {
            if (animeProperties.ContainsKey("translateX") && animeProperties.ContainsKey("translateY")) {
                animeProperties["margin"] = new ThicknessAnimeProperty((TranslateAnimeProperty)animeProperties["translateX"],(TranslateAnimeProperty)animeProperties["translateY"]);
                animeProperties.Remove("translateX");
                animeProperties.Remove("translateY");
            }

            foreach (var prop in animeProperties) {
                yield return prop.Value;
            }
        }

        //public IEnumerable<FrameworkElement> getTargets() {
        public FrameworkElement[] getTargets() {
            if (targets == null) {
                return new FrameworkElement[] { target };
            } else /*if (targets != null)*/ {
                return targets;
                //foreach (FrameworkElement el in targets) {
                //    yield return el;
                //}
            }
        }
    }
}
