/**
* JetBrains Space Automation
* This Kotlin-script file lets you automate build activities
* For more info, see https://www.jetbrains.com/help/space/automation.html
*/

job("Hello World!") {
    // Run the job in a default instance in cloud.
    requirements {
        workerPool = WorkerPools.SPACE_CLOUD
    }

    // Run the steps in parallel.
    parallel {

        // Say hello from a worker.
        host("Hello Host") {
            shellScript {
                content = """
                echo "Hello from a host!"
                """
            }
        }

        // Say hello from an Ubuntu container.
        container(displayName = "Hello Ubuntu", image = "ubuntu:latest")
        {
            args("echo", "Hello from a container!")
        }
    }
}
