Pugrad
======

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

How To Install
--------------

This package uses the [scoped registry] feature to resolve package dependencies.
Please add the following sections to the manifest file (Packages/manifest.json).

[scoped registry]: https://docs.unity3d.com/Manual/upm-scoped.html

To the `scopedRegistries` section:

```
{
  "name": "Keijiro",
  "url": "https://registry.npmjs.com",
  "scopes": [ "jp.keijiro" ]
}
```

To the `dependencies` section:

```
"jp.keijiro.pugrad": "1.0.0"
```

After changes, the manifest file should look like below:

```
{
  "scopedRegistries": [
    {
      "name": "Keijiro",
      "url": "https://registry.npmjs.com",
      "scopes": [ "jp.keijiro" ]
    }
  ],
  "dependencies": {
    "jp.keijiro.pugrad": "1.0.0",
...
```

How To Use
----------

<!--4567890123456789012345678901234567890123456789012345678901234567890123456-->

To cerate a colormap, select Assets -> Create -> Pugrad. It creates a `.pugrad`
file that generates a `Texture2D` asset procedurally.

![inspector](https://i.imgur.com/KfkVl7Nm.jpg)

You can change the resolution of the texture on the inspector. You can also
change the lightness of the gradient when using the HSLuv colormap.
