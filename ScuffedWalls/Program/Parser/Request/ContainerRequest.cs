﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScuffedWalls
{
    public class ContainerRequest : Request
    {
        public const string WorkspaceKeyword = "workspace";
        public const string DefineKeyword = "function";
        public string Name { get; private set; }
        public List<FunctionRequest> FunctionRequests { get; private set; } = new List<FunctionRequest>();
        public List<VariableRequest> VariableRequests { get; private set; } = new List<VariableRequest>();

        private CacheableScanner<Parameter> _paramScanner;
        public override Request Setup(List<Parameter> Lines)
        {
            Parameters = new TreeList<Parameter>(Lines, Parameter.Exposer);
            DefiningParameter = Lines.First();
            UnderlyingParameters = new TreeList<Parameter>(Lines.Lasts(), Parameter.Exposer);
            Name = DefiningParameter.StringData;

            _paramScanner = new CacheableScanner<Parameter>(UnderlyingParameters);
            Type previous = Type.None;

            while (_paramScanner.MoveNext())
            {
                bool varIs = VariableRequest.IsName(_paramScanner.Current.Clean.Name);
                bool funIs = FunctionRequest.IsName(_paramScanner.Current.Clean.Name);
                if (varIs || funIs)
                {
                    addLastRequest();
                    previous = varIs ? Type.VariableRequest : funIs ? Type.FunctionRequest : Type.None;
                }
                _paramScanner.AddToCache();
            }
            addLastRequest();
            return this;

            void addLastRequest()
            {
                switch (previous)
                {
                    case Type.FunctionRequest:
                        if (_paramScanner.AnyCached)
                            FunctionRequests.Add((FunctionRequest)new FunctionRequest().Setup(_paramScanner.GetAndResetCache()));
                        break;
                    case Type.VariableRequest:
                        if (_paramScanner.AnyCached)
                            VariableRequests.Add((VariableRequest)new VariableRequest().Setup(_paramScanner.GetAndResetCache()));
                        break;
                }
            }
        }
    }
}
