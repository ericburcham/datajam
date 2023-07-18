val buildContainerImage = "ubuntu:22.04"
val buildScript = """
    apt-get update && apt-get install -y apt-utils apt-transport-https
    apt-get install -y curl unzip wget software-properties-common git

    wget https://dot.net/v1/dotnet-install.sh
    chmod +x ./dotnet-install.sh
    ./dotnet-install.sh --channel 6.0
    PATH=${'$'}PATH:${'$'}HOME/.dotnet:${'$'}HOME/.dotnet/tools
    dotnet --list-sdks
    
    dotnet nuget add source {{ project:msa_nuget_space_target_url }} -n space -u "%JB_SPACE_CLIENT_ID%" -p "%JB_SPACE_CLIENT_SECRET%" --store-password-in-clear-text

    ./build.sh
""".trimIndent()

job("Continuous Integration Build") {
    startOn {
        gitPush {
            enabled = true

            // Enable for all git flow branches.
            branchFilter {
                -"refs/bugfix/*"
                -"refs/feature/*"
                -"refs/hotfix/*"
                -"refs/release/*"
                -"refs/support/*"
            }
        }
    }

    container(buildContainerImage) {
        env["FEED_URL"] = "{{ project:msa_nuget_space_target_url }}"

        resources {
            cpu = 2.cpu
            memory = 4.gb
        }

        // Disable unneeded dotnet stuff.
        env.set("DOTNET_NOLOGO", "true")
        env.set("DOTNET_SKIP_FIRST_TIME_EXPERIENCE", "true")
        env.set("DOTNET_CLI_TELEMETRY_OPTOUT", "true")

        // Set environment variables.
        env.set("NuGetSpaceTargetApiKey", "{{ project:msa_nuget_space_api_key }}")
        env.set("NuGetSpaceTargetUrl", "{{ project:msa_nuget_space_target_url }}")

        shellScript {
            content = buildScript
        }
    }
}
