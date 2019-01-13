using NetMMS.Model;
using System;
using System.Collections.Generic;

namespace NetMMS.Design {
    public static class SpriteFactory {
        static readonly Dictionary<Type, Func<SpriteData, ISpriteVM>> _map;

        static SpriteFactory() {
            _map = new Dictionary<Type, Func<SpriteData, ISpriteVM>> {
                {typeof (Computer), x => new ComputerVM(new Computer(), x)},
                {typeof (Printer), x => new PrinterVM(new Printer(), x)}
            };
        }

        public static ISpriteVM Create(Type typeEntity, SpriteData data) {
            if (_map.ContainsKey(typeEntity)) {
                return _map[typeEntity](data);
            }
            throw new Exception($"unexpected type: {typeEntity}");
        }
    }
}
