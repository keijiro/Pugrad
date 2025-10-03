# Pugrad

![photo](https://i.imgur.com/t8uvAy7l.jpg)

**Pugrad** is a color gradient generator for Unity that provides a set of widely
used, perceptually uniform colormaps.

Currently, Pugrad includes the following colormaps:

- Viridis, Plasma, Magma, and Inferno from the [Matplotlib colormaps]
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

To create a colormap, select Assets > Create > Pugrad. This creates a `.pugrad`
file that generates a `Texture2D` asset procedurally.

![inspector](https://i.imgur.com/KfkVl7Nm.jpg)

You can change the texture resolution in the Inspector. When you use the HSLuv
colormap, you can also adjust the gradient lightness.
