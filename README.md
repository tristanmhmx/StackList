# StackList
Code for a custom list of stack layouts for Xamarin Forms mainly thought for rezisable cells

###Setup
*Install PullToRefreshLayout Control in your solution

###Usage

```xaml
<controls:StackListControl ItemsSource="{Binding YOUR_DATA}" RefreshCommand="{Binding YOUR_COMMAND}" x:Name="CardListControl" IsRefreshing="{Binding ISREFRESHING_FLAG}">
              <controls:StackListControl.ItemTemplate>
                <DataTemplate>
                  <controls:YOUR_CONTROL />
                </DataTemplate>
              </controls:StackListControl.ItemTemplate>
            </controls:StackListControl>
```
License
Licensed under MIT, see license file.  // you may not use this file except in compliance with the License. // Unless required by applicable law or agreed to in writing, software // distributed under the License is distributed on an "AS IS" BASIS, // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. // See the License for the specific language governing permissions and // limitations under the License. //
