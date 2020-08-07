# SVG to XAML

This converter app uses [vvvv's SVG library](https://github.com/vvvv/SVG) to parse the SVG xml. It then converts the svg elements to their corresponding XAML element, places them on a canvas, inside of a `Viewbox`, and then inside of a `ControlTemplate`. These are all added to a `ResourceDictionary.xaml` and can be used easily in buttons.
