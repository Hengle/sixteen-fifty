#!/usr/bin/env python

import glob
import os
import sys
import tempfile

#recursive glob - looks for all files below src_path that match 'flt' and aren't in 'excl'
def rglob(src_path, flt, excl=[]):
    import fnmatch
    matches = []
    for root, dirnames, filenames in os.walk(src_path):
        for filename in fnmatch.filter(filenames, flt):
            include_it = True
            fullpath = os.path.join(root, filename)
            for exclusion in excl:
                if fnmatch.fnmatch(fullpath, exclusion):
                    include_it = False
                    break
            if include_it:
                matches.append(fullpath)
    return matches

def get_main_files(filedir, inexcl):
    excl = list(inexcl)
    excl.append("*/Lib/*")
    excl.append("*/NGui/*")
    files = []
    files += rglob(os.path.join(filedir, "Assets/Scripts"), "*.cs", excl)
    files += rglob(os.path.join(filedir, "Assets/Standard Assets/Image Effects (Pro Only)"), "*.cs", excl)
    return files
    
def get_lib_files(filedir, inexcl):
    excl = list(inexcl)
    excl.append("*/Build/*")
    files = []
    files += rglob(os.path.join(filedir, "Assets/Lib"), "*.cs", excl)
    files += rglob(os.path.join(filedir, "Assets/NGui"), "*.cs", excl)
    return files

#if entry 'match' is in 'file_list', swap it with 'swap'
def swap_file(file_list, match, swap):
    if match!=None and swap!=None:
        for c1 in xrange(len(file_list)):
            val = file_list[c1]
            if os.path.abspath(val)==os.path.abspath(match):
                file_list[c1] = swap
                return True
        
    return False

MONO_DIR = "/opt/Unity/Editor/Data/MonoBleedingEdge/lib/mono/4.5"

def compile_files(files):
    files_as_string = " ".join(
        '"' + f + '"' for f in files)

    compile_command = " ".join([
        "mono /opt/Unity/Editor/Data/MonoBleedingEdge/lib/mono/4.5/csc.exe",
        "/noconfig /nologo /warn:4 /debug:+ /debug:full ",
        "/optimize- /nowarn:0169 /fullpaths /utf8output /t:library",])

    with tempfile.NamedTemporaryFile("w+", delete=False) as tmp:
        tmp.write(r
