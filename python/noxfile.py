"""Build automation for DataJam Python - equivalent to NUKE build system."""

import os
import shutil
from pathlib import Path

import nox

# Python versions to test against (GitHub Actions supports these)
PYTHON_VERSIONS = ["3.10", "3.11", "3.12"]
PROJECT_ROOT = Path(__file__).parent
SRC_DIR = PROJECT_ROOT / "src"
TESTS_DIR = PROJECT_ROOT / "tests"
ARTIFACTS_DIR = PROJECT_ROOT / "artifacts"

# Pinned tool versions for consistency across all environments
LINT_TOOLS = [
    "ruff==0.8.0",
    "black==24.10.0",
    "mypy==1.13.0",
    "isort==5.13.2",
]

nox.options.error_on_missing_interpreters = True


@nox.session(name="clean")
def clean(session: nox.Session) -> None:
    """Clean build artifacts and temporary files."""
    session.log("Cleaning build artifacts...")

    # Clean artifacts directory
    if ARTIFACTS_DIR.exists():
        shutil.rmtree(ARTIFACTS_DIR)

    # Clean Python cache files
    for pattern in ["**/__pycache__", "**/*.pyc", "**/*.pyo", "**/.pytest_cache"]:
        for path in PROJECT_ROOT.glob(pattern):
            if path.is_dir():
                shutil.rmtree(path)
            else:
                path.unlink()

    # Clean build directories
    for pattern in ["build", "dist", "*.egg-info"]:
        for path in PROJECT_ROOT.glob(pattern):
            if path.exists():
                if path.is_dir():
                    shutil.rmtree(path)
                else:
                    path.unlink()


def _run_lint_tools(session: nox.Session) -> None:
    """Unified linting function used by all lint sessions."""
    session.install(*LINT_TOOLS)

    session.log("Running ruff...")
    session.run("ruff", "check", str(SRC_DIR), str(TESTS_DIR))

    session.log("Running black...")
    session.run("black", "--check", str(SRC_DIR), str(TESTS_DIR))

    session.log("Running isort...")
    session.run("isort", "--check-only", str(SRC_DIR), str(TESTS_DIR))

    session.log("Running mypy...")
    session.run("mypy", str(SRC_DIR))


@nox.session(name="lint")
def lint(session: nox.Session) -> None:
    """Run linting and code style checks."""
    _run_lint_tools(session)


@nox.session(name="format")
def format_code(session: nox.Session) -> None:
    """Format code using black and isort."""
    session.install(*LINT_TOOLS)

    session.log("Running ruff --fix...")
    session.run("ruff", "check", "--fix", str(SRC_DIR), str(TESTS_DIR))

    session.log("Running black...")
    session.run("black", str(SRC_DIR), str(TESTS_DIR))

    session.log("Running isort...")
    session.run("isort", str(SRC_DIR), str(TESTS_DIR))


@nox.session(python=PYTHON_VERSIONS, name="test")
def test(session: nox.Session) -> None:
    """Run unit tests."""
    session.install("-e", ".[testing]")

    # Run unit tests only (no integration tests)
    session.run(
        "pytest",
        str(TESTS_DIR / "unit"),
        "--cov=src",
        "--cov-report=term-missing",
        "--cov-report=xml",
        *session.posargs,
    )


@nox.session(python=PYTHON_VERSIONS, name="test-integration")
def test_integration(session: nox.Session) -> None:
    """Run integration tests (requires Docker for TestContainers)."""
    session.install("-e", ".[testing,sqlalchemy]")

    # Set environment for Oracle TestContainer
    session.env["TESTCONTAINERS_ORACLE_PASSWORD"] = "oracle123"

    # Run integration tests
    session.run(
        "pytest",
        str(TESTS_DIR / "integration"),
        "--cov=src",
        "--cov-report=term-missing",
        "--cov-report=xml",
        "-v",
        *session.posargs,
    )


@nox.session(name="test-all")
def test_all(session: nox.Session) -> None:
    """Run all tests (unit and integration)."""
    session.install("-e", ".[testing,sqlalchemy]")

    # Set environment for Oracle TestContainer
    session.env["TESTCONTAINERS_ORACLE_PASSWORD"] = "oracle123"

    # Run all tests
    session.run(
        "pytest",
        str(TESTS_DIR),
        "--cov=src",
        "--cov-report=term-missing",
        "--cov-report=xml",
        "--cov-report=html",
        *session.posargs,
    )


@nox.session(name="build")
def build(session: nox.Session) -> None:
    """Build distribution packages."""
    session.install("build")

    # Create artifacts directory
    ARTIFACTS_DIR.mkdir(exist_ok=True)

    session.log("Building distribution packages...")
    session.run("python", "-m", "build", "--outdir", str(ARTIFACTS_DIR))


@nox.session(name="publish")
def publish(session: nox.Session) -> None:
    """Publish packages to PyPI (requires PYPI_TOKEN environment variable)."""
    session.install("twine")

    pypi_token = os.getenv("PYPI_TOKEN")
    if not pypi_token:
        session.log("PYPI_TOKEN environment variable not set. Skipping publish.")
        return

    session.log("Publishing packages to PyPI...")
    session.run(
        "twine",
        "upload",
        "--username", "__token__",
        "--password", pypi_token,
        f"{ARTIFACTS_DIR}/*",
    )


@nox.session(name="dev-setup")
def dev_setup(session: nox.Session) -> None:
    """Setup development environment."""
    session.install("-e", ".[dev,testing,sqlalchemy]")
    session.install("pre-commit")

    session.log("Installing pre-commit hooks...")
    session.run("pre-commit", "install")


@nox.session(name="datajam")
def datajam_build(session: nox.Session) -> None:
    """Main build target - equivalent to NUKE's main target."""
    session.log("Running DataJam Python build pipeline...")

    # Clean first
    clean(session)

    # Install dependencies
    session.install("-e", ".[dev,testing,sqlalchemy]")

    # Run linting using unified function
    _run_lint_tools(session)

    # Run tests
    test_all(session)

    # Build packages
    build(session)

    # Publish if token is available
    publish(session)

    session.log("DataJam Python build completed successfully!")


# Convenience aliases
@nox.session(name="ci")
def ci(session: nox.Session) -> None:
    """CI pipeline - runs tests and builds without publishing."""
    session.log("Running CI pipeline...")

    # Install dependencies
    session.install("-e", ".[dev,testing,sqlalchemy]")

    # Run linting using unified function
    _run_lint_tools(session)

    # Run all tests
    test_all(session)

    # Build packages
    build(session)

    session.log("CI pipeline completed successfully!")