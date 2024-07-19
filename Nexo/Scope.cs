using Nexo.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexo
{
    public sealed class Scope
    {
        private Dictionary<string, Value> _variables;
        private readonly Scope? _parent;

        public Scope()
        {
            _variables = [];
        }

        public Scope(Scope parnet)
        {
            _variables = [];
            _parent = parnet;
        }

        private Scope? GetScope(string name)
        {
            if (_variables.ContainsKey(name))
            {
                return this;
            }

            if (_parent == null)
            {
                return null;
            }

            return _parent.GetScope(name);            
        }

        public void Declare(string name, Value value)
        {
            _variables.Add(name, value);
        }

        public Value Get(string name)
        {
            var scope = GetScope(name);
            if (scope == null)
            {
                throw new Exception("Not declareted");
            }            
            return scope._variables[name];
        }
        
        public void Set(string name, Value value)
        {
            var scope = GetScope(name);
            if (scope == null)
            {
                throw new Exception("Not declareted");
            }
            scope._variables[name] = value;
        }
    }
}
