# StackList
Code for a custom list of stack layouts for Xamarin Forms mainly thought for rezisable cells

###Setup
*Install PullToRefreshLayout Control in your solution

###Usage

Instead of ObservableCollection use SmartCollection

```xaml
<controls:StackListControl 
  ItemsSource="{Binding YOUR_DATA}" 
  RefreshCommand="{Binding YOUR_COMMAND}" 
  IsRefreshing="{Binding ISREFRESHING_FLAG}">
    <controls:StackListControl.ItemTemplate>
        <DataTemplate>
            <controls:YOUR_CONTROL />
        </DataTemplate>
    </controls:StackListControl.ItemTemplate>
</controls:StackListControl>
```
The MIT License (MIT)

Copyright (c) 2016 Alset & Tristan Martinez

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
