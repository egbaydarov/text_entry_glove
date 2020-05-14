////-----------------------------------------------------------------------
//// <copyright file="controllerproviderfactory.cs" company="google inc.">
//// copyright 2016 google inc. all rights reserved.
////
//// licensed under the apache license, version 2.0 (the "license");
//// you may not use this file except in compliance with the license.
//// you may obtain a copy of the license at
////
////     http://www.apache.org/licenses/license-2.0
////
//// unless required by applicable law or agreed to in writing, software
//// distributed under the license is distributed on an "as is" basis,
//// without warranties or conditions of any kind, either express or implied.
//// see the license for the specific language governing permissions and
//// limitations under the license.
//// </copyright>
////-----------------------------------------------------------------------
using UnityEngine;

/// @cond
namespace Gvr.Internal
{
    /// factory that provides a concrete implementation of icontrollerprovider for the
    /// current platform.
    static class ControllerProviderFactory
    {
        /// provides a concrete implementation of icontrollerprovider appropriate for the current
        /// platform. this method never returns null. in the worst case, it might return a dummy
        /// provider if the platform is not supported. for demo purposes the emulator controller
        /// is returned in the editor and in standalone buids, for use inside the desktop player.
        static internal IControllerProvider CreateControllerProvider(GvrControllerInput owner)
        {
            // use emualtor in editor, and in standalone builds (for demo purposes).
#if unity_editor
            // use the editor controller provider that supports the controller emulator and the mouse.
            return new editorcontrollerprovider(owner.emulatorconnectionmode);

#elif unity_android
            if (androidnativeshimcontrollerprovider.shimavailable())
            {
                // use the gvr unity shim api.
                return new androidnativeshimcontrollerprovider();
            }
            else
            {
                debug.logwarning(
                    "gvr unity shim not found. creating dummy controller provider.");
                return new dummycontrollerprovider();
            }
#else
            // platform not supported.
            Debug.LogWarning("no controller support on this platform.");
            return new DummyControllerProvider();
#endif  // unity_editor || unity_android
        }
    }
}

/// @endcond
