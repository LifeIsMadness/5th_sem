#include <iostream>
#include <vector>
#include <string>
#include <malloc.h>
#include <time.h>

using namespace std;

_int16* mmx_func(_int8* A, _int8* B, _int8* C, _int16* D);
_int8* rand_int8(size_t size);
_int16* rand_int16(size_t size);

constexpr size_t SIZE = 8;
//ctrl + k, c/u
int main()
{
    srand(time(0));

    _int8* A = rand_int8(SIZE),
        * B = rand_int8(SIZE),
        * C = rand_int8(SIZE);
    _int16* D = rand_int16(SIZE);

    _int16* F = mmx_func(A, B, C, D);
   
    cout << "result MMX = ";
    for (size_t i = 0; i < SIZE; i++)
    {
        cout << F[i] << ',';
    }

    cout << endl;
    cout << "result = ";
    for (size_t i = 0; i < SIZE; i++)
    {
        cout << A[i] * B[i] + C[i] - D[i] << ',';
    }

    return 0;
}

_int8* rand_int8(size_t size)
{
    auto* mas = new _int8[size];
    for (size_t i = 0; i < size; i++)
    {
        mas[i] = rand() % 256 - 128;
    }

    return mas;
}

_int16* rand_int16(size_t size)
{
    auto* mas = new _int16[size];
    for (size_t i = 0; i < size; i++)
    {
        mas[i] = rand() % 65536 - 32768;
    }

    return mas;
}

__declspec(naked) _int16* mmx_func(_int8* A, _int8* B, _int8* C, _int16* D)
{
    __asm
    {
        pushad
        mov eax, [esp + 32 + 4]
        mov ebx, [esp + 32 + 8]
        mov ecx, [esp + 32 + 12]
        mov edx, [esp + 32 + 16]

        movq mm0, qword ptr[eax]
        movq mm1, qword ptr[ebx]
        movq mm2, qword ptr[ecx]
        movq mm3, [edx]

        //mm6 is filled with zeros
        // if mm6's zero byte > than mm0's byte then it will be FF in a result

        pcmpgtb mm6, mm0
        punpcklbw mm0, mm6
        pcmpgtb mm6, mm1
        punpcklbw mm1, mm6
        pcmpgtb mm6, mm2
        punpcklbw mm2, mm6

        pmullw mm0, mm1
        paddsw mm0, mm2
        psubsw mm0, mm3
        //store the result
        movq mm4, mm0
        // edx + 8 for next 
        movq mm0, [eax]
        movq mm1, [ebx]
        movq mm2, [ecx]
        movq mm3, [edx + 8]

        pcmpgtb mm6, mm0
        punpckhbw mm0, mm6
        pcmpgtb mm6, mm1
        punpckhbw mm1, mm6
        pcmpgtb mm6, mm2
        punpckhbw mm2, mm6

        pmullw mm0, mm1
        paddsw mm0, mm2
        psubsw mm0, mm3

        movq mm5, mm0
        //memory allocating
        popad
        push 16
        call dword ptr[malloc]
        add esp, 4
        movq[eax], mm4
        movq[eax + 8], mm5
        ret
    }
}