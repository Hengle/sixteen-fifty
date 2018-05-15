#!/usr/bin/env python

from PIL import Image, ImageDraw
import colorsys
import sys

V = [20, 30, 45, 58, 70, 78, 85, 92, 100]
S = [30, 40, 52, 65, 75, 63, 45, 30, 15]
H = [350, 30, 60, 100, 140, 180, 220, 265]

H_ADJ = 8

C_ADJ = 40

assert(len(V) == len(S))

def main(dst):
    W = len(V) * 2 - 1
    im = Image.new('RGB', (W, len (H)))

    for y, h in enumerate(H):
        for i, (s, v) in enumerate(zip(S, V)):
            if (i - 4) < 0:
                BLUE_H = 240
                if h < 240 - 180:
                    BLUE_H -= 360

                w = (i - 4) / C_ADJ
                hh = (BLUE_H * w + (1 - w) * h)
            else:
                RED_H = 0
                if h > 180:
                    RED_H = 180
                w = (i - 4) / C_ADJ
                hh = (RED_H * w + (1 - w) * h)

            hh += (i - 4) * H_ADJ
                    
            (r, g, b) = colorsys.hsv_to_rgb(hh/360, s/100, v/100)            

            im.putpixel((i, y), (int (r * 255), int (g *255), int (b * 255)))

            (r, g, b) = colorsys.hsv_to_rgb(hh/360, s/300, v/100)

            im.putpixel((W - 1 - i, y), (int(r * 255), int(g * 255), int(b * 255)))

    im.save(dst)

if __name__ == '__main__':
    dst = sys.argv[1]
    main(dst)
