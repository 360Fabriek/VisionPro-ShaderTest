//
// This custom View is referenced by SwiftUISampleInjectedScene
// to provide the body of a WindowGroup. It's part of the Unity-VisionOS
// target because it lives inside a "SwiftAppSupport" directory (and Unity
// will move it to that target).
//

import Foundation
import SwiftUI
import UnityFramework

struct HelloWorldContentView: View {
    var body: some View {
        VStack {
            Text("Hello, SwiftUI!")
            Button("Spawn Red Object") {
                CallCSharpCallback("spawn red")
            }
            Button("Spawn Green Object") {
                CallCSharpCallback("spawn green")
            }
            Button("Spawn Blue Object") {
                CallCSharpCallback("spawn blue")
            }
        }
        .onAppear {
            // Call the public function that was defined in SwiftUISamplePlugin
            // inside UnityFramework
            CallCSharpCallback("appeared")
        }
    }
}

#Preview(windowStyle: .automatic) {
    HelloWorldContentView()
}

