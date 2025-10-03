# Pugrad

![photo](https://i.imgur.com/t8uvAy7l.jpg)

**Pugrad** is a color gradient generator for Unity that supports commonly-used
perceptually uniform colormaps.

At the moment, Pugrad supports the following colormaps:

- [Matplotlib colormaps] (Viridis, Plasma, Magma, Inferno)
- [Turbo]
- [HSLuv] (repeatable)

![screenshot](https://i.imgur.com/tyj9wIpl.jpg)

[Matplotlib colormaps]: https://bids.github.io/colormap/
[Turbo]: https://ai.googleblog.com/2019/08/turbo-improved-rainbow-colormap-for.html
[HSLuv]: https://www.hsluv.org/

## Installation

The Pugrad package (`jp.keijiro.pugrad`) can be installed via the "Keijiro"
scoped registry using Package Manager. To add the registry to your project,
please follow [these instructions].

[these instructions]:
  https://gist.github.com/keijiro/f8c7e8ff29bfe63d86b888901b82644c

## How To Use

To cerate a colormap, select Assets -> Create -> Pugrad. It creates a `.pugrad`
file that generates a `Texture2D` asset procedurally.

![inspector](https://i.imgur.com/KfkVl7Nm.jpg)

You can change the resolution of the texture on the inspector. You can also
change the lightness of the gradient when using the HSLuv colormap.
