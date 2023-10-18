using System;

namespace Stac.Api.Package
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ForceAssemblyReference: Attribute
    {        
        public ForceAssemblyReference(Type forcedType)
        {
            //not sure if these two lines are required since 
            //the type is passed to constructor as parameter, 
            //thus effectively being used
            Action<Type> noop = _ => { };
            noop(forcedType);
        }
    }

}
