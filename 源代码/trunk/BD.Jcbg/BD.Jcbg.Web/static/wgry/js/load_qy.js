var tempCity = "浙江省温州市";

var page = 1; //页面page，1，2，3
var refreshflg1 = true; //页面1刷新flg
var refreshflg2 = true; //页面2刷新flg
var refreshflg3 = true; //页面3刷新flg

//第一页所需参数
var map; //百度地图
var tabId = "GcList"; //地图页tab区分
var gclistAll = new Array();
var qylistAll = new Array();
var rylistAll = new Array();
var refreshmaptabflg1 = true; //tab1刷新flg
var refreshmaptabflg2 = true; //tab2刷新flg
var refreshmaptabflg3 = true; //tab3刷新flg
var defimg = '/9j/4AAQSkZJRgABAQEASABIAAD/2wBDAAYEBQYFBAYGBQYHBwYIChAKCgkJChQODwwQFxQYGBcUFhYaHSUfGhsjHBYWICwgIyYnKSopGR8tMC0oMCUoKSj/2wBDAQcHBwoIChMKChMoGhYaKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCj/wAARCAMgAiwDASIAAhEBAxEB/8QAHAABAQACAwEBAAAAAAAAAAAAAAEGBwIEBQMI/8QAShABAAEEAQAFBwgGBwgABwAAAAECAwQRBQYSITFRBxNBYXGBkRQiMpKhscHRI1JTVGJyFUJEk7LC8DM0Q2NzgqLhFiQ2ZHSD0v/EABQBAQAAAAAAAAAAAAAAAAAAAAD/xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwD9LibNgomzYKJs2CibNgomzYKJs2CibNgomzYKJs2CibNgomzYKJs2CibNgomzYKJs2CibNgomzYKJs2CibNgomzYKJs2CibNgomzYKJs2CibNgomzYKJs2CibNgomzYKJs2CibNgomzYKJs2CibNgomzYIIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogAIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCiAKIAogCbNoAuzaALs2gC7NoAuzaALs2gC7NoAuzaALs2gC7NoAuzaALs28Dlel3C8bum7m0Xbsf8Ox8+fZuOyPfLEuS8plczNPGYFNMb7K8ire/+2O74g2Zt8snJsYtvr5V+1Zo/WuVxTHxlpHP6X87mzPX5C7ap/Vsfo9e+O37XhXLld2ua7tdVdc99VU7mQbyy+mHA4szFfI2q5j0Wom59tMTDx8nykcRbmYsWMy9Pj1aaYn4zv7GowGyb/lP74scV7Jrv/hFP4uhd8pfKTP6LDwqI/iiqr/NDBQGYXPKJzlX0fktH8tr85l8aun3SCe7JtR7LNP5MVAZPPTrpDv8A3+I//Tb/AP5WOnnSGP7bTPts0fkxcBldHT/n6e+/Zq9tmn8HYo8o3N0/St4Vf81ur8KoYYAzyz5TOSj/AG2Fh1/ydan75l3bXlPnsi9xPtmnI/DqtbANsWPKXxdX+3w8y3P8MU1R98PSx+nvAXpiKsq5Zmf2lqr8IlpUBv7G6Q8Pk68zyeJMz3RN2KZ+E9r0rdyi5RFduqmuie6qmdxL83vpYvXbFfXsXa7VXjRVNM/YD9HbNtD4vSrnMWf0XJ5FX/Vq85/i29vD8o3L2dRkWsXIj0zNE01fZOvsBt3Ztr7D8pmLV2ZvHX7XrtVxX9+nvYXTTgcvUU51Nquf6t6maNe+ez7QZHs2+ONk2Mq318W/avUfrW64qj4w+oLs2gC7NoAuzaALs2gC7NoAuzaALs2gC7NoAuzaALs2gC7NoAmzYAbNgBs2AGzYAbNgBs2AGzYAbNgBs2AG3G7dos2q7l6um3bojdVdU6iI8Zl0ub5bE4bBqys651aI7KaY7aq58Ij0y050n6T53P3pi7VNnEid0Y9E/Nj1z4z6/hoGbc/5RMbGmqzw9qMq5HZ525uLceyO+fs97AOX6Q8py8zGdmXK7c/8KmerR9WOyfe8oAAAAAAAAAAAAAAAAAAAAAAAABytXK7VyK7VdVFcd1VM6mPe9zB6X87hTHm+Qu3KfTTe1c376ty8EBs3g/KPbuVU2uZx4tTP/GsxM0++nvj3b9jPsTKsZmPRfxb1F6zV3V0TuJfnR6PDczn8Nf8AO8fkVW9/Sontor9sd34g3/s2wbg/KJg5MUW+Vt1Yl6eyblMTVbmfvj7fazWxetZFmm7YuUXbVcbproqiqJ9kwD6bNgBs2AGzYAbNgBs2AGzYAbNgBs2AGzYAgmzYKJs2CibNgomzYKJs2CibNgomzYKJs2CvP5zlsbhePry8yrVMdlNEd9dXoiHbyci1i49y/kVxbs26ZqqqnuiIaQ6Wc9e5/kpvVboxqN02bU/1afGfXPp/9A63P8zlc5n1ZOZV6rduJ+bbp8IeaAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD1eC5/kODuzVgXtUVTuu1XG6KvbH4xqXlANp8R5RsTIuU2+Sxq8XfZ52irr079ca3EfFnFi9ayLNF7HuUXbVcbproncTHtfnR7PRzpFncDf62LX17FU7uWK5+bV+U+uPt7gb2HkdHefwuexfOYlfVu0x+ks1fSon8Y9b1tgomzYKJs2CibNgomzYKJs2CibNgomzYIJs2CibNgomzYKJs2CibNgomzYKJs2CibY7006R0cFgTTaqpqz70atUd/V/jn1R9s+8GKeU/n/PX/6Ixa/0VuYqyJifpVd8U+7v9vsYA5V11XK6q7lU1V1TM1VTO5mfGXEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHYwMzI4/LoycO7VavUTuKqf8AXbHqbg6H9KrHPWfNXerZ5CiN1W/RXH61P5ehpd9Ma/dxsi3fx7lVu9bnrU10zqYkH6JHgdDefp57i/OV6py7Wqb1Md2/RVHqn83vbBRNmwUTZsFE2bBRNmwUTZsFE2bAE2bBRNmwUTZsFE2bBRNmwUTZsFE2A6HPcna4fir+bejrebj5tO9daqeyI+LRnI52RyObdysu5Ny9cncz4eqPUzDyo8zGVn2+MsVbtY3zrmvTcmO73R98+DBgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAev0X5q5wXLW8qjdVqfmXaI/rUT3++O+PY3hjX7WVj27+PXFyzcpiqmqO6Yl+eGX9Bulc8Nc+R501VcfXO4nvm1PjHq8Y98enYbdHzs3aL1qm7ZrpuW643TXTO4mPGJc9gomzYKJs2CibNgomzYKJs2AOIDkOIDkOIDkOIDkOIDkOIDk8TpdzdHB8RXfiYnJufMs0z6avH2R3/Z6XstM9PeUq5LpDfpirdjGmbNuI7uz6U++d9vhEAx65XXduV3LlU111zNVVUzuZme+XEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAdvF5LOxLc28TNybFuZ3NNu7VTHwiWaeTPkuTyuYvWb2RfyMSLU1V+dqmqKJ3GtTPdPf2e3wYA3N0By8XK6OY8Ytui1csxFu9RTER8+I+lPjvsnYMlHEByHEByHEByHEByHEA2bQBdm0AXZtAF2bQBdm0AXZtAGN9O+dq4XiYpx51l5O6Lc/qx6avduPfMNNtg+VyjV7i7m/pU3Kdezq/m18AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA9zofzlXB8tTdqmqcW7qi/THh6KvbH5x6XhgP0PRXTXRTXRVFVFUbiqJ3Ex4uW2B+THm5yMavisirdyxT17Mz6aN9se6Z+E+pnYLs2gC7NoAuzaALs2gCbNgBs2AGzYAbNgBs2AGzYA1h5VsuLvK4mLT2+YtTXPqmqe74Ux8WDvd6c3pvdK+Qqn+rXFEe6mI/B4QAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAO9wnI3OJ5THzbMbqtVdtP61M9kx8Nt54eVazMW1k49XWtXaYrpn1S/PzYnkt5eZi9xV6rcUxN2zue7t+dT9u/iDYezYAbNgBs2AGzYA4hs2AGzYAbNgBs2AGzYAbcL12izZuXbk6oopmqqfCIjcg0h0nr850j5Or/7m5HwqmHmPpkXar+RdvV/SuVzXPtmdvmAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA9nodf8Ak3Sjja963di39b5v4vGc7Nyqzet3bc6roqiqmfXEg/QQ+WLkUZONZv2u23doiun2TG4fXYAbNgBs2AGzYIJs2CibNgomzYKJs2CibNgrEfKNzNODxM4NquPlOVGpiJ7abfpn393x8GW7aJ5vLu53LZeRkTM113J7PCInUR7o7AdIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAG3PJ1n/ACzo5RaqndzFqm1Ps74+yde5lDVvkvzJs81fxZnVGRa3EeNVM7j7JqbR2CibNgomzYKJs2AOIDkOIDkOIDkOIDkOIDzukPM2ODwPlORE1zNUUUW6e+qWl+Qu27/IZV6zFUWrl2quiKu+ImZmNs+6bcNzPN8vbpxseJw7NHVorquUxEzPbVOt78I7vQwPlMG7xufdxMiaJu2piKponcdsRP4g6oAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAPZ6HXpsdKOOrj03ep9aJp/Fupo3o5Ez0h4zX71an/AMobxkFHEByHEByHEA2bQBdm0AXZtAF2bQBdm0AXbTPTad9KuQn+OP8ADDcrTPTP/wCqOR/6n4QDxQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAZd5NuNjL5ivLuT8zEiKojxqq3EfdM/BtPbBfJVRrAz6/G7THwj/2zkF2bQBdm0AXZtAE2bADZsANmwA2bADZsANtNdNI10p5D+eJ/8Yblag6e09XpXneE9SY+pSDHwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAbX8nGPNjo1TcmP9vdquR7Oyn/KyjbzOjPU/wDh3jfNa6vyejevHXb9u3pgbNgBs2AGzYA4hs2AGzYAbNgBs2AGzYDVflIt9TpLNX7SzRV98fg2ptrnypWtchg3tfTtVUfVnf8AmBhAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAANj+THkK72Fk4NydxYmK7c+qre4+Pb72bPF6I8XRxXC2KOpFORdpi5enXbNU9up9nc9rYAbNgBs2AGzYIGzYAbNgBs2AGzYAbNgMI8qVreBg3tfQu1UfWjf+Vm+2I+U3U8BY/wDyaf8ADWDWIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADv8AA4sZvNYWPVHWpru0xVHjTvc/Zt0GQdBL2LY6RWbuZdptRTTVFuau6a5jWt+jsmQbcDZsANmwA2bADZsEEAUQBRAFEAUQBWD+VDKpjEwsSJ+fVcm7MeERGo/xT8Ga3K6bduq5cqimimJqqqnuiI9LTPSHk6uX5a9lTuLcz1bdM/1aI7vz9syDzQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAZp0K6UV4923x/I3Jqx6tU2rlU9tufRE/w/d7O7Y7Qkt48bXVc47EuVTM1VWqKpmfTMxAO0IAogCiAJs2gC7NoAuzaALs2gC7NoAxPyi8lOLxVGHanVzKn52p7qI7/AIzqPi1mybyh35u9JK7cz2WbdFER7Y63+ZjIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADdvCzP9DYG+/5Pb39WGkm7eJjXFYUeFij/AAwDubNoAuzaALs2gBs2gC7NoAuzaALs2gC7TYA1J03nfSnO/wCz/BS8NkHTyiaOlGXM91cUVR9SI/Bj4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEt44EdXBxqfC1TH2NHT3N626epbpp/ViIBz2bQBdm0AXZtAE2bTZsF2bTZsF2bTZsF2bTZsF2bTZsGAeUzDmnKxM2I+bXRNqr1TE7j47n4MJbg6T8fHJcJk2Ip612Kevb1Hb1o7Y17e73tPgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA52KPOX7dH61UU/GW85ntaW4WjznM4FE91WRbifrQ3RsF2bTZsF2bTZsF2bTZsEDZsANmwA2bADZsANmwGoOlGF8g53LsxGrc1dejw6tXb9nd7m39sE8pmJ24eZTHjZqn7af8wMGAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB6/RG157pJgU+FfW+ETP4NutY+T2z5zpDFf7K1VX91P+Zs7YAbNgBs2AGzYIJs2CibNgomzYKJs2CibNgrxemGJ8r6O5dOo61unztMz6Or2z9m/i9nbjXTFdFVFcbpqjUx4wDR4+2bj1YuZfx6/pWrlVE+6dPiAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADOPJnj/OzsiY/Vt0z8Zn8Gdsd6CYs43R61VO4qv1VXZifhH2RDIdgomzYKJs2CibNgggCiAKIAogCiAKIA1j08xfk/SG5XEapv0U3I9vdP2xv3sdZ/wCUjF6+FiZUR2265tz2eiqN/wCX7WAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAPriWK8rKs49r6d2uKI9szp8mXeTzjvPZ1zPuU/o7EdWj11zH4R98Az/Gs0Y2PasWuy3aoiinfhEah9EAUQBRAFEANm0AXZtAF2bQBdm0AXZtAF2bQB5vSTE+XcHmWIiZr6nXpiI7etT2xH2aaibvai6R4UcfzWVYpjVuKutRqOyKZ7Yj3b17geaAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACxE1TEREzM9kRDb/A4EcZxOPjajr007rmPTVPbLXfQ3BqzedsTNMzasT52ufDXd9uvtbTBdm0AXZtAF2bQBdm0ATZtAF2bQBdm0AXZtAF2bQBdm0AXbAfKNi1U52LlR9Cu35uezumJ3+P2M9eZ0j46OU4m9YiI87Hz7c/xR3fHtj3g1KExMTMTExMdkxIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA++Fi3s3Jox8a3Ny7XOoiPvn1O/wvA5vLVRNmjzdjfber7Kfd4z7PsbF4Th8XiLHUx6etcqj592r6VX5R6gTo7xFvh8GLVOqr1fzrtzX0p8PZH+u96m0AXZtAF2bQBdm0AXZtAEAAAAAAAAAAAAABrzpzw9WLmTn2Kd49+d16/qV/8Avv8Abv1MWbpvWrd+1XavU012641VTVG4mGE830Nqp697iq+tT3+Yrntj+WfT7/jIMMHK5brtXKrdymqiumdVU1RqYn2OIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMn4bojez8W1k3cq3atXI61MU0zVVr192mT8d0V4zDmKq7U5Ncem9O4j3d3x273R+nqcHx8f8AIon40xL0AIiIiIiIiI7IiAAAAAAAAAAAAQTZsFE2bBRNmwUTZsFE2bBRNmwUTZsFE2bB5XK8Dg8pkWr+TRVFyjvmiddePCf9beN034izTxVrIxLNFv5NMU1RRGvmT/718ZZdt8suxRlYt6xcj5l2iaJ98A00Od61VYvXLVyNV26poqjwmJ04AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA+li3N6/btR311RTHvkG3+Pt+a4/Ft/qWqafhEOwnd3GwUTZsFE2bBRNmwUTZsFE2bBRNmwQQBRAFEAUQBRAFEAUQBRAFEAa26b4nybnbldMRFF+mLkaj090/bG/e8BnnlCxevgY+THfarmifZVH5xHxYGAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA9ToxY+Uc/g0eFzr/AFfnfg8tlHk/x5ucrevzHzbVvW/XM9n2RINgiAKIAogCiAKIAogCiAAgCiAKIAogCiAKIAogCiAKIA6PPYvyzhsyxETNVVuZpiPTVHbH2xDUzc7UnM4vyPlcrHiNU0XJ6sfwz2x9kwDpgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAANg9AMXzXE3L8xqq/c7J8aaeyPt6zX8RNUxERMzPZEQ25xmLGFx+PjRr9HRFMzHpn0z8dg7YgCiAKIAogCiAKIAogCbNoAuzaALs2gC7NoAuzaALs2gC7NoAuzaALs2gC7YF0/xYtclYyaYiIvUan11U+n4THwZ41t0u5SOS5Lq2at49iOpRMd1U+mf9eAPDAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB7PRHC+Wc3Z60bt2f0tXu7vt02btjfQfA+TcZOTXGrmRO/ZTHd+M++GRguzaALs2gC7NoAuzaALs2gC7NoAuzaAIAAAAAAAAAAAAAAAADxukXN2uKsTTRMV5dcfMo8PXPq+/7g6HTLmvktmcHGr/T3I/STH9SmfR7Z+72wwNzu3K712u5dqmu5XPWqqnvmXAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB2+Jw5z+RsY1M685V2z4RHbM/DbqMu8n2PFV/LyZjtopi3T7+2fugGaW6Kbdumi3EU0UxFNMR3REOQAAAAAAAAAAAAAAAgmzYKJs2CibNgomzYKJs2CibNgo+GXl4+Ha85lXaLVHjVPf7PFi/J9MaKZmjjrXXn9rcjUe6O/wCOvYDLqqoopmqqYppiNzMz2Q8PP6Ucdi7pouVZFcdmrUbj493w2wTP5HLz6t5d+u54U91MeyI7HUBkfIdLc7IiacamjGon9X51Xxn8mPXK6rldVdyqquuqdzVVO5mXEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAZf5P8mmm7l41U6rriK6fXrsn74Yg+2Hk3cPKt5Firq3Lc7iQbdHS4nkbPJ4VGRZ7Jnsro3201eDubBRNmwUTZsFE2bBRNmwUTZsFE2bBRNmwQQBRAFEAUQBRHQ5PlsTjaN5N35+txbp7ap935g7125Rat1XLtUUUUxuqqZ1EQxHl+l3fa4un1eerj7o/P4PG5znsnlJm3/ssbe4t0z3+2fS8cH1yci9lXZu5F2u5cn01Tt8gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB6HC8pe4rLi7a+dbnsuW/RVH5+Etk4GZZzsai/jV9a3V8Ynwn1tTO/w/KZHF5HnLE7on6duZ7Ko/P1g2kPP4jlsblLPWsVauR9K1VPzqf/Xrd8FEAUQBRAFEAUQBRAAQBRAFEAV88i/axrNV3IuU27dPfVVOnU5flLHF4/nL87qnsotx31T+Xra85blMnk73XyKtUx9G3T9Gn/XiD2+Z6V3bs1WuNibVvum7VHzp9nh9/sYxXXVcrqruVVV11Tuaqp3My4gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAOdq5XZuU3LVdVFdPbFVM6mPeyPj+l2VZpijMtU5ER/WierV7/RLGQGxMDpPx2VVFFVdWPXP7WNR8e746e5ExMbidxLT71+F57K4yqKNzdxvTaqnu9k+gGyR1OOz8fkceL2NX1o9NM99M+Ew7QKIAogCiAKIAmzaALs2gC7eRzfO43G0VUxNN3J7ot0z3T/F4O5k8hh4sT5/Js0THomuN/Dva95yvDu5929hXrlyLlc11dajURM9vZPfPbv0QDq52Xezsmq/k1zXcq+ER4R6nwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHZ47Ov8fk038avq1R3x6Ko8JjwbG4blbHKY3nLXza6eyu3M9tM/l62sHZ4/NvYGVTfx6tV098eiqPCfUDauzbp8Vn2eSw6b9mdb7KqZ76Z8HbBdm0AXZtAF2bQBHj8l0jwcKZoiub92P6tvtiPbPc8DpXzF+5l3cKzVNuxbnq16ntrn079Xq/1GNAyHM6V516dY9NvHp9UdafjPZ9jx8jOy8nfn8m9cifRVXMx8HWAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAehwvJ3eLy4u0bqtVdlyj9aPzbHxci3lY9F6xXFVuuNxMNUPY6O8xXxmR1bm6sWufn0+E/rQDYmzbjauUXbdNy3VFVFUbpqjumHIDZsANmwBq/mauty+bP/Pr/AMUum7PJzvksufG9X/il1gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAe70b5yrj7kWMiZqxKp9s258Y9Xq/1Oe0VxXRTVRVFVNUbiYncTDUrIui/OfIq4xcqr/5aqfm1T/w5/IGc7Nps2C7Nps2DVmdO83InxuVfe+D6ZM7ybs/xz975gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAzroZl15HG127tXWmzV1ad/q67I+977XPAcnVxmbFc7mzXqm5T6vH2w2JRXTcoproqiqmqNxMTuJgHINmwapv8A+2ufzT97g5Xe25V7XEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABkfRXmYxaoxMqrViqfmVT/AFJ8J9UscAbYGK9Fub68UYWXX8+Oy1XPp/hn1+DKdg1VX9KXFZ70AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAZv0f5e5f4+PlEde5RV1OtvtqjUds+vtYQyHo7/uVz/qT90Ax+e9Ce8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAe7wMzGHXr9pP3Q8J7XCTrFr/AJ5+6AeKAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA9fiJ1jVfzz90PIepxc6x6v5vwgHlz3iz3oAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA9HjpmLNWv1vwh5zt4tc029RPpB1avpSjlX9KXEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB97M6ofB9rc6pB87vZcqj1uLnf7L9z+afvcAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH1t/RfJypnsByyf8Aebv88/e+b65fZl34/jq+98gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFiexF0D7Z8azsmPC7V974Ozyca5LLj/AJ1f3y6wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADnRG4cH2txukH15eNcplRP7WqftdRkHL8NmX+QvXrFuK7dc7j50R6I8XRng+Rj+zf+dP5g80ehPDchH9mq91UT+LhPFZ8f2W78AdIdyeMzo78W99XaTx2b+63/AO7kHUHa/o/N/dMj+7n8k+QZn7pkf3VX5A6w7HyHL/dMj+7k+RZX7rf/ALuQdcff5Jk/u176kp8lyP3e99SQfEfb5NkfsLv1JT5Nf/YXfqSD5D6/J7/7G79SU8xe/ZXPqyD5j6eYu/srn1ZPM3f2Vf1ZB8xz81c/Z1/Vk81c9NFUe2AcBz83X+rKdSrwkHEcupV4SdSrwkHEcurPgnVnwBBerPgdWfAEF6s+BqfAEF1JqQQXUmpBANAC6k1IILqTUggvVnwOrPgCC9WfA6s+AIL1Z8F6lXgDiOXUq8JOpV4SDiOfm6v1ZPN1+imZ9wOA5+aufs6/qyeZu/s6/qyDgPp5m7+yr+rJ5i9+yufVkHzH1jHvT3Wbn1ZPk9/9jd+pIPkPr8mv/sLv1JX5LkfsLv1JB8R9vkmT+73vqSvyPK/dr/8AdyD4DsfIsv8Adb/93K/IMz90yP7ufyB1h2f6PzP3TI/uqvyX+js390yP7uQdUduONzZ/st/6kuUcXnfut36oOkO9HEZ892Lc9+nOOF5Cf7NP16fzB5z0uNx/PWKqtb1Vr7IWOC5Gf7Pr/vp/NkXB8dViYU0ZEU+cqrmrUTvXZEfgD//Z';

//第二页所需参数
var loop;
var millisec = 60000;
var firstflg = true;

//第三页所需参数
var listtabId = "gctab"; //tab区分
var refreshlisttabflg1 = true; //tab1刷新flg
var refreshlisttabflg2 = true; //tab2刷新flg
var refreshlisttabflg3 = true; //tab3刷新flg
//var qybh = "";
$(function() {

    //全局初始化
    init();

    //获取省份
    GetProvince(qybh);

    //地图初始化
    initMap(qybh);

    //页面加载
    //pageLoad();

    //各选项changes事件绑定
    bindChange();

    // $("#tabTitle").children().eq(0).click();
    $(".all_num").css("line-height", $('.all_num').height() + "px");
    $(".all_title").css("line-height", $('.all_title').height() + "px");

});

//全局初始化
function init() {
    //政府端
    if (type == 1) {
        // $("#province10").prop('disabled', true);
        // $("#city10").prop('disabled', true);
    } else {
        $("#selectQylx").prop('disabled', true);
        $("#selectQy").prop('disabled', true);    
    }
    // var obj = document.getElementById("qybh");
    // if (obj != null)
    //     qybh = obj.innerHTML;

    //鼠标滚动全屏
    $('.panel').css({
        'height': $(window).height()
    });
    $.scrollify({
        section: 'section',
        scrollbars: false,
        before: function() {
            var sectionName = $.scrollify.current()[0].dataset.sectionName;
            if (sectionName == "section1") {
                page = 1;
            }
            if (sectionName == "section2") {
                page = 2;
            }
            if (sectionName == "section3") {
                page = 3;
            }
            Refresh();
        }
    });

    //加载固定块(回到顶部按钮)
    // layui.use('util', function () {
    //     var util = layui.util;
    //     util.fixbar({
    //         // bar1: true,
    //         click: function (type) {
    //             console.log(type);
    //             if (type === 'bar1') {
    //                 alert('点击了bar1')
    //             }
    //         }
    //     });
    // });
}


//全局变量初始化
function initvariable() {

    clearInterval(loop);

    $("#gcpageindex").val("1");
    $("#qypageindex").val("1");
    $("#rypageindex").val("1");

    $("#listcontent").empty();
    $("#qycontent").empty();
    $("#rycontent").empty();

    $("#txtGcmc").val("");
    $("#txtQymc").val("");
    $("#txtGcmcForRy").val("");
    $("#txtRymc").val("");
    $("#txtLableGcmc").val("");
    $("#txtLableQymc").val("");
    $("#txtLableGcmcForRy").val("");
    $("#txtLableRymc").val("");

    refreshflg1 = true; //页面1刷新flg
    refreshflg2 = true; //页面2刷新flg
    refreshflg3 = true; //页面3刷新flg

    //第一页所需参数
    gclistAll = new Array();
    qylistAll = new Array();
    rylistAll = new Array();
    refreshmaptabflg1 = true; //tab1刷新flg
    refreshmaptabflg2 = true; //tab2刷新flg
    refreshmaptabflg3 = true; //tab3刷新flg

    //第二页所需参数
    firstflg = true;

    //第三页所需参数
    refreshlisttabflg1 = true; //tab1刷新flg
    refreshlisttabflg2 = true; //tab2刷新flg
    refreshlisttabflg3 = true; //tab3刷新flg
}
var onceTime = true;
//刷新
function Refresh() {

    //参数改变
    formChange();

    //统计页
    if (page == 1 && refreshflg1) {
          refresh2();
    }

    //地图页面
    if (page == 2 && refreshflg2) {
        
      
        clearInterval(loop);
        refresh1();
    }


    //一览页面
    if (page == 3 && refreshflg3) {

        if (onceTime) {
            $("#tabTitle").children().eq(0).click();
            onceTime = false;
        }
        clearInterval(loop);
        refresh3();

    }
}

//第二图刷新
function refresh2() {

    
    setHW();
    statistics1();
    statistics2();
    statistics4(qybh);

    loop = setInterval(function() {

        setHW();
        statistics1();
        statistics2();
        statistics4();

    }, millisec);
}

window.onresize = function() {
    //高度调整
    setHW();
}

//统计图表宽高调整
function setHW() {

    //每个图表的宽度
    // var bottomwidth = ($("#enginner_chart").width() - 60) / 3;
    // $("#enginner_1").css("width", bottomwidth);
    // $("#enginner_2").css("width", bottomwidth);
    // $("#enginner_3").css("width", bottomwidth);
    // $("#enginner_4").css("width", ($(".panel").width() - 80));

    //每个图表的高度
    // var bottomheight = ($(".panel").height() - 140) / 2;
    // $("#enginner_1").css("height", bottomheight);
    // $("#enginner_2").css("height", bottomheight);
    // $("#enginner_3").css("height", bottomheight);
    // $("#enginner_4").css("height", bottomheight);
}



//第三图刷新
function refresh3() {

    $("#item").height($("#panel3").height() - 100);
    $("#tabTitle").find('.active').click();
}

//展开更多检索按钮
// function btnLableSearch(flg) {

    // if (flg == 1) {
    //     initGcTable();
    // }

    // if (flg == 2) {
    //     initQyTable();
    // }

    // if (flg == 3) {
    //     initRyTable();
    // }
// }

// function togczs(gcbh, gcmc) {

//     //每个图表的宽度
//     var w = ($(".panel").width() * 0.95).toString() + "px";
//     //每个图表的高度
//     var h = ($(".panel").height() * 0.95).toString() + "px";

//     var index = layer.open({
//         type: 2,
//         content: '/welcome/welcomegcqp?gcbh=' + gcbh + '&gcmc=' + gcmc,
//         area: [w, h]
//     });

//     //layer.full(index);
// }



// function toqy(sfzhm, gcbh) {

//     //每个图表的宽度
//     var w = ($(".panel").width() * 0.95).toString() + "px";
//     //每个图表的高度
//     var h = ($(".panel").height() * 0.95).toString() + "px";

//     var index = layer.open({
//         type: 2,
//         content: '/welcome/welcomeryz?sfzhm=' + sfzhm + '&gcbh=' + gcbh,
//         area: [w, h]
//     });

// }

// function tory(sfzhm, gcbh) {

//     //每个图表的宽度
//     var w = ($(".panel").width() * 0.95).toString() + "px";
//     //每个图表的高度
//     var h = ($(".panel").height() * 0.9).toString() + "px";

//     var index = layer.open({
//         type: 2,
//         content: '/welcome/welcomeryz?sfzhm=' + sfzhm + '&gcbh=' + gcbh,
//         area: [w, h]
//     });
// }

var type = 1; //政府段：1  企业端：2



$.ajax({
    type: "GET",
    url: "/welcome/GetGCXZ",
    dataType: "json",
    success: function(data) {
        var str = "";

        if (data && data.length) {
            for (var i = 0, len = data.length; i < len; i++) {
                str += "<option value='" + data[i].lxmc + "'>" + data[i].lxmc + "</option>";
            }
        }

        $("#gcxz").append(str);
    }
});

//第一图
function statistics1() {

    ajaxTpl("/wzwgry/GetGC_QYFBTJ?qybh="+qybh, paramsData, function(data) {
        var name = [];
        var value = [];
        var legendName = [];
        var maxVal = 0;
        for (var i = 0, len = data.Datas.length; i < len; i++) {
            data.Datas[i].value = parseFloat(data.Datas[i].value);
            maxVal += data.Datas[i].value;
        }

        for (var i = 0, len = data.Datas.length; i < len; i++) {
            data.Datas[i].name = data.Datas[i].name == "" ? "其他" : data.Datas[i].name;
            name.push(data.Datas[i].name);
            legendName.push(name[i] + "  " + (data.Datas[i].value * 100 / maxVal).toFixed(2) + '%');

            value.push(data.Datas[i].value);
        }

        var option = {
            title: {
                text: '工程区域分布统计',
                top: '3%',
                left: '4%',
            },
            tooltip: {
                trigger: 'item',
                formatter: "{b}: {c} ({d}%)"
            },
            legend: {
                type: 'scroll',
                orient: 'vertical',
                top: '10%',
                right: '10%',
                bottom: 20,
                selectedMode: false,

                itemWidth: 10,
                data: data.Datas,
                formatter: function(val) {
                    for (var i = 0, len = name.length; i < len; i++) {
                        if (val == name[i]) {
                            return legendName[i];
                        }
                    }
                    return val;
                }
            },
            series: [{
                type: 'pie',
                radius: ['20%', '60%'], //内环 外环宽度
                center: ['36%', '56%'], //中点位置
                avoidLabelOverlap: false,
                label: {
                    normal: {
                        show: false
                    }
                },
                labelLine: {

                    normal: {
                        show: false
                    }
                },
                data: data.Datas

            }],
            color: ['#5674d6', '#1ecdbf', '#ea5d67', '#f7b96b', '#91c3f2', '#40c484', '#ff9570', '#84eac0', '#75a7ec']
        };
        var doc = document.getElementById('enginner_1');
        echarts.init(doc).setOption(option);
        doc.style.background = "white";
    });
}

//第二图
function statistics2() {

    ajaxTpl("/wzwgry/GetTjlist?qybh="+qybh, paramsData, function(data) {

        if (data.Datas && data.Datas[0]) {
            statistics0Data(data); //八块内容
            statistics2Data(data); //第二图
            statistics3Data(data); //第三图
        }
    });
}

function statistics0Data(data) {
    var tmp = data.Datas[0];
    var jzmj = (tmp.jzmj/10000).toFixed(2) + '万方' ;
    var gczj = (tmp.gczj/10000).toFixed(2) + '亿元' ;

    $("#enginner_qyzs").html(tmp.sgdwnum+ "个"); //施工单位数
    $("#enginner_zgcs").html(tmp.zgcs + "个"); //总工程数
    $("#enginner_jzmj").html(jzmj); //建筑面积
    $("#enginner_gczj").html(gczj); //工程造价
    $("#enginner_zcry").html(tmp.zcry + "个"); //在册人员
    $("#enginner_yfje").html(tmp.yfje+ "元"); //应发金额
    $("#enginner_sfje").html(tmp.sfje+ "元"); //实发金额
    $("#enginner_ljffje").html(tmp.ljffje+ "元"); //累计发放金额
}


function statistics2Data(data) {
    var name = [];
    var value = [];
    var legendName = [];

    var ary = [];
    var tmp = data.Datas[0];


    name.push("实名制总人数");
    ary.push({
        name: "实名制总人数", //在册人员
        value: 0
    });
    legendName.push("实名制总人数  " + tmp.zcry);

    if (tmp.zzry) { //在职人员
        name.push('在职人员');
        value.push(tmp.zzry);
        ary.push({
            name: "在职人员", //在职减去在岗
            value: parseFloat(tmp.zzry) - parseFloat(tmp.dqry)
        });
        legendName.push("在职人员  " + tmp.zzry);
    }

    if (tmp.dqry) { //在岗人员
        name.push('在岗人员');
        value.push(tmp.dqry);
        ary.push({
            name: "在岗人员",
            value: tmp.dqry
        });
        legendName.push("在岗人员  " + tmp.dqry);
    }


    var option = {
        title: {
            text: '在册人员统计',
            top: '3%',
            left: '4%'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{b}: {c} ({d}%)"
        },
        legend: {
            orient: 'vertical',
            top: '10%',
            right: '10%',
            itemWidth: 10,
            selectedMode: false,
            data: [{ name: '实名制总人数' }, {
                name: '在职人员',
                icon: 'image://../static/wgry/images/icon.jpg'
            }, {
                name: '在岗人员'
            }],
            formatter: function(val) {
               
                for (var i = 0, len = name.length; i < len; i++) {
                    if (val == name[i]) {
                        return legendName[i];
                    }
                }
                return val;
            }
        },
        series: [{

            type: 'pie',
            radius: ['20%', '60%'], //内环 外环宽度
            center: ['36%', '56%'], //中点位置
            avoidLabelOverlap: false,
            label: {
                normal: {
                    show: false
                }
            },
            labelLine: {
                normal: {
                    show: false
                }
            },
            data: ary

        }],
        color: ['#fff', '#ea5d67', '#5674d6']
    };
    var doc = document.getElementById('enginner_2');
    echarts.init(doc).setOption(option);
    doc.style.background = "white";
}

function statistics3Data(data) {
    var name = [];
    var value = [];
    var legendName = [];

    var ary = [];
    var tmp = data.Datas[0];
    if (tmp.yfje) {
        name.push("应发金额");
        value.push(tmp.yfje);
        ary.push({
            name: "应发金额",
            value: tmp.yfje
        });
        legendName.push("应发金额 " + tmp.yfje);
    }
    if (tmp.sfje) {
        name.push("实发金额");
        value.push(tmp.sfje);
        ary.push({
            name: "实发金额",
            value: tmp.sfje
        });
        legendName.push("实发金额 " + tmp.sfje);
    }

    var option = {
        title: {
            text: '工资发放统计',
            top: '3%',
            left: '4%',
        },
        tooltip: {
            trigger: 'item',
            formatter: "{b}: {c} ({d}%)"
        },
        legend: {
            orient: 'vertical',
            top: '10%',
            right: '10%',
            itemWidth: 10,
            data: ary,
            selectedMode: false,
            formatter: function(val) {
                for (var i = 0, len = name.length; i < len; i++) {
                    if (val == name[i]) {
                        return legendName[i];
                    }
                }
                return val;

            }
        },
        series: [{
            type: 'pie',
            radius: ['20%', '60%'], //内环 外环宽度
            center: ['36%', '56%'], //中点位置
            avoidLabelOverlap: false,
            label: {
                normal: {
                    show: false
                }
            },
            labelLine: {

                normal: {
                    show: false
                }
            },
            data: ary

        }],
        color: ['#5674d6', '#ea5d67']
    };
    var doc = document.getElementById('enginner_3');
    echarts.init(doc).setOption(option);
    doc.style.background = "white";

}


//第四图


function getTmpParams() {
    var province = $("#province10").val();
    var city = $("#city10").val();
    var district = $("#district10").val() || ' ';
    var jd = $("#street10").val() || ' ';
    var qy = $("#selectQy").val() || ' ';

    // 省|市|取|街道|企业名|工程名|工程性质
    var gcm = $("#selectGczt").val()|| ' ';
    var gcxz = $("#gcxz").val() || ' ';
    var ary = [province, city, district, jd, qy, gcm, gcxz];

    return ary.join("|");
}



$("#tabTitle").on("click", "li", function() {
    var that = $(this);
   
    var i = that.index();
    that.addClass("active").siblings().removeClass("active");
    that.parent().next().children().eq(i).show().siblings().hide();

    var params = getTmpParams();

    switch (i) {
        case 0:
            initQyTable1(params,qybh);
            break;
        case 1:
            initGcTable1(params, qybh);
            break;
        case 2:
            initRyTable1(params, qybh);
            break;
        default:
            break;
    }
});