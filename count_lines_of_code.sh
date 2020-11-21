#!/bin/bash

# Source: https://stackoverflow.com/a/1358573/11768869
find ./Assets/ProjectAssets/ -name '*.cs' -o -name '*.shader' | xargs wc -l