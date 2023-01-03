#!/bin/bash
SCRIPT_NAME="recipe.cake"

echo "Restoring .NET Core tools"
dotnet tool restore

echo "Running Build"
dotnet cake $SCRIPT_NAME "$@"