﻿using ModChart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace ScuffedWalls
{

    static class Internal
    {
        public static string[] toUsableCustomData(this Parameter[] parameter)
        {
            List<string> customDatas = new List<string>();
            foreach (var p in parameter)
            {
                customDatas.Add(p.parameter + ":" + p.argument);
            }
            return customDatas.ToArray();
        }
        public static T[] GetAllBetween<T>(this T[] mapObjects, float starttime, float endtime)
        {
            return mapObjects.Where(obj => (Convert.ToSingle(typeof(T).GetProperty("_time").GetValue(obj, null).ToString()) >= starttime) && (Convert.ToSingle(typeof(T).GetProperty("_time").GetValue(obj, null).ToString()) <= endtime)).ToArray();
        }
        static public bool MethodExists<t>(this string methodname)
        {
            foreach (var methods in typeof(t).GetMethods())
            {
                if (methods.Name == methodname) return true;
            }
            return false;
        }
        static public Parameter[] TryGetParameters(this string[] args)
        {
            List<Parameter> parameters = new List<Parameter>();
            foreach (var line in args)
            {
                try
                {
                    string parameter = line.Split(':', 2)[0].ToLower();
                    string argument = line.Split(':', 2)[1];
                    while (argument.Contains("Random("))
                    {
                        Random rnd = new Random();
                        string[] asplit = argument.Split("Random(", 2);
                        string[] randomparam = asplit[1].Split(',');
                        argument = asplit[0] + (Convert.ToSingle(rnd.Next(Convert.ToInt32(Convert.ToSingle(randomparam[0]) * 100f), Convert.ToInt32((Convert.ToSingle(randomparam[1].Split(')')[0]) * 100f) + 1))) / 100f) + asplit[1].Split(')', 2)[1];
                        
                    }
                    parameters.Add(new Parameter { parameter = parameter, argument = argument });
                }
                catch(Exception e)
                {
                    if (e is IndexOutOfRangeException) throw new ScuffedException($"Error parsing \"{line}\", Missing Colon?");
                    else throw new ScuffedException($"Error parsing line\"{line}\"");
                }
                
            }

            return parameters.ToArray();
        }

        public static T DeepClone<T>(this T a)
        {
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(a));
        }


        public static string removeWhiteSpace(this string WhiteSpace)
        {
            return new string(WhiteSpace.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }

        //adjust for lower caseeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
        public static BeatMap.CustomData CustomDataParse(this string[] CustomNoodleData)
        {
            BeatMap.CustomData CustomData = new BeatMap.CustomData();
            BeatMap.CustomData.Animation Animation = new BeatMap.CustomData.Animation();

            foreach (var _customObject in CustomNoodleData)
            {
                string[] _customObjectSplit = _customObject.Split(':');

                if (_customObjectSplit[0] == "AnimateDefinitePosition".ToLower()) Animation._definitePosition = JsonSerializer.Deserialize<object[][]>($"[{_customObjectSplit[1]}]");

                else if (_customObjectSplit[0] == "AnimatePosition".ToLower()) Animation._position = JsonSerializer.Deserialize<object[][]>($"[{_customObjectSplit[1]}]");

                else if (_customObjectSplit[0] == "scale".ToLower()) CustomData._scale = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);

                else if (_customObjectSplit[0] == "track".ToLower()) CustomData._track = JsonSerializer.Deserialize<object>($"\"{_customObjectSplit[1]}\"");

                else if (_customObjectSplit[0] == "color".ToLower()) CustomData._color = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);

                else if (_customObjectSplit[0] == "NJSOffset".ToLower()) CustomData._noteJumpStartBeatOffset = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);

                else if (_customObjectSplit[0] == "NJS".ToLower()) CustomData._noteJumpMovementSpeed = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);

                else if (_customObjectSplit[0] == "AnimateDissolve".ToLower()) Animation._dissolve = JsonSerializer.Deserialize<object[][]>($"[{_customObjectSplit[1]}]");

                else if (_customObjectSplit[0] == "AnimateDissolveArrow".ToLower()) Animation._dissolveArrow = JsonSerializer.Deserialize<object[][]>($"[{_customObjectSplit[1]}]");

                else if (_customObjectSplit[0] == "AnimateColor".ToLower()) Animation._color = JsonSerializer.Deserialize<object[][]>($"[{_customObjectSplit[1]}]");

                else if (_customObjectSplit[0] == "AnimateRotation".ToLower()) Animation._rotation = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");

                else if (_customObjectSplit[0] == "AnimateLocalRotation".ToLower()) Animation._localRotation = JsonSerializer.Deserialize<object[][]>($"[{_customObjectSplit[1]}]");

                else if (_customObjectSplit[0] == "AnimateScale".ToLower()) Animation._scale = JsonSerializer.Deserialize<object[][]>($"[{_customObjectSplit[1]}]");

                else if (_customObjectSplit[0] == "interactable".ToLower()) CustomData._interactable = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);

                else if (_customObjectSplit[0] == "rotation".ToLower()) CustomData._rotation = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);

                else if (_customObjectSplit[0] == "LocalRotation".ToLower()) CustomData._localRotation = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);

                else if (_customObjectSplit[0] == "fake".ToLower()) CustomData._fake = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);

                else if (_customObjectSplit[0] == "position".ToLower()) CustomData._position = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);

                else if (_customObjectSplit[0] == "cutDirection".ToLower()) CustomData._cutDirection = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);

                else if (_customObjectSplit[0] == "NoSpawnEffect".ToLower()) CustomData._disableSpawnEffect = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);


                CustomData._animation = Animation;

            }

            return CustomData;

        }

        //adjust for lowercaseeaaa
        public static BeatMap.CustomData.CustomEvents.Data CustomEventsDataParse(this string[] CustomNoodleData)
        {

            BeatMap.CustomData.CustomEvents.Data Data = new BeatMap.CustomData.CustomEvents.Data();

            foreach (var _customObject in CustomNoodleData)
            {
                string[] _customObjectSplit = _customObject.Split(':');


                if (_customObjectSplit[0] == "track".ToLower()) Data._track = JsonSerializer.Deserialize<object>("\"" + _customObjectSplit[1] + "\"");

                else if (_customObjectSplit[0] == "parentTrack".ToLower()) Data._parentTrack = JsonSerializer.Deserialize<object>("\"" + _customObjectSplit[1] + "\"");

                else if (_customObjectSplit[0] == "duration".ToLower()) Data._duration = JsonSerializer.Deserialize<object>(_customObjectSplit[1]);

                else if (_customObjectSplit[0] == "easing".ToLower()) Data._easing = JsonSerializer.Deserialize<object>("\"" + _customObjectSplit[1] + "\"");

                else if (_customObjectSplit[0] == "AnimateDefinitePosition".ToLower()) Data._definitePosition = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");

                else if (_customObjectSplit[0] == "AnimateDissolve".ToLower()) Data._dissolve = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");

                else if (_customObjectSplit[0] == "childTracks".ToLower()) Data._childrenTracks = JsonSerializer.Deserialize<object[]>(_customObjectSplit[1]);

                else if (_customObjectSplit[0] == "AnimateDissolveArrow".ToLower()) Data._dissolveArrow = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");

                else if (_customObjectSplit[0] == "AnimateColor".ToLower()) Data._color = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");

                else if (_customObjectSplit[0] == "AnimateRotation".ToLower()) Data._rotation = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");

                else if (_customObjectSplit[0] == "AnimatePosition".ToLower()) Data._position = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");

                else if (_customObjectSplit[0] == "AnimateLocalRotation".ToLower()) Data._localRotation = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");

                else if (_customObjectSplit[0] == "AnimateScale".ToLower()) Data._scale = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");

                else if (_customObjectSplit[0] == "AnimateTime".ToLower()) Data._time = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");

                else if (_customObjectSplit[0] == "isInteractable".ToLower()) Data._interactable = JsonSerializer.Deserialize<object[][]>("[" + _customObjectSplit[1] + "]");


            }
            return Data;
        }
    }
    public struct Parameter
    {
        public string parameter;
        public string argument;
    }

}
