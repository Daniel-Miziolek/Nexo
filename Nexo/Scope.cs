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
        private readonly Dictionary<string, (Value, bool)> _variables;
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

        public void Declare(string name, Value value, bool isConstant)
        {
            _variables.Add(name, (value, isConstant));
        }

        public Value Get(string name)
        {
            var scope = GetScope(name);
            return scope == null ? throw new Exception("Not declareted") : scope._variables[name].Item1;
        }

        public void Set(string name, Value value)
        {
            var scope = GetScope(name) ?? throw new Exception("Not declareted");
            if (scope._variables[name].Item2)
            {
                throw new Exception("Cannot assign to constant");
            }
            scope._variables[name] = (value, false);
        }
    }
}
