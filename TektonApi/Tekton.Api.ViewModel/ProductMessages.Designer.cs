﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tekton.Api.ViewModel {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ProductMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ProductMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Tekton.Api.ViewModel.ProductMessages", typeof(ProductMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La inserción no pudo ser procesada..
        /// </summary>
        public static string EXCEPCION_INSERT {
            get {
                return ResourceManager.GetString("EXCEPCION_INSERT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Se ha presentado un error no controlado: {0}.
        /// </summary>
        public static string EXCEPCION_NO_CONTROLADA {
            get {
                return ResourceManager.GetString("EXCEPCION_NO_CONTROLADA", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La actualización no pudo ser procesada..
        /// </summary>
        public static string EXCEPCION_UPDATE {
            get {
                return ResourceManager.GetString("EXCEPCION_UPDATE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Se ha presentado un error de validación: {0}.
        /// </summary>
        public static string EXCEPCION_VALIDACION {
            get {
                return ResourceManager.GetString("EXCEPCION_VALIDACION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La consulta no generó resultados..
        /// </summary>
        public static string SIN_DATOS {
            get {
                return ResourceManager.GetString("SIN_DATOS", resourceCulture);
            }
        }
    }
}
